using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Whaledevelop.Dialogs;
using Whaledevelop.Extensions;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    public delegate void DialogChangeNodeDelegate(DialogProfile dialogProfile);

    public delegate void DialogFinishedDelegate(string finishBranchId);

    public class DialogNodeGraphProcessor
    {
        private readonly string _dialogId;
        private readonly NodeEdge[] _edges;

        private readonly DialogNode[] _nodes;
        private DialogNode _currentNode;

        private DialogProfile _dialogProfile;

        public DialogNodeGraphProcessor(IDialogSettings dialogSettings, string dialogId)
        {
            _dialogId = dialogId;
            _nodes = dialogSettings.Graph.CopyNodes();
            _edges = dialogSettings.Graph.Edges;
        }

        public event DialogChangeNodeDelegate onChangeNode;
        public event DialogFinishedDelegate onFinish;

        public void Start()
        {
            BuildGraph();

            if (_nodes.FirstOrDefault(node => node is StartDialogNode) is not StartDialogNode startDialogNode)
            {
                throw new("There isn't found startDialogNode");
            }
            _dialogProfile = new(_dialogId);
            StartNode(startDialogNode);
        }

        public void EndDialogNode(string nodeId)
        {
            if (nodeId != _currentNode.NodeId)
            {
                //_logger.Warning($"There is try to end dialog node {nodeId}, but already node is {_currentNode.NodeId}");
                return;
            }

            if (_currentNode is IOneDirectionNode node)
            {
                if (_currentNode is EndDialogNode endDialogNode)
                {
                    Finish(endDialogNode.FinishBranchId);
                    return;
                }

                if (node.NextNode == null)
                {
                    Finish(null);
                    return;
                }

                StartNode(node.NextNode);
            }
            else
            {
                //_logger.Error($"Current node {_currentNode} is not of type {nameof(IOneDirectionNode)}");
            }
        }

        public void ChooseDialogOption(int optionIndex)
        {
            if (_currentNode is not BranchingDialogNode branching)
            {
                //_logger.Error("There is no current branching");
                return;
            }
            var optionText = branching.Options[optionIndex];
            var optionNode = GetOptionNode(branching, _edges, _nodes, optionText);
            if (optionNode == null)
            {
                //_logger.Error($"There is no option {optionText} in branching {branching.NodeId}");
                return;
            }
            StartNode(optionNode);
        }

        public DialogNode GetOptionNode(BranchingDialogNode branchingDialogNode, IEnumerable<NodeEdge> edges, IEnumerable<BaseNode> nodes, string optionText)
        {
            var portName = NodeGraphUtility.GetPortName(optionText);
            return (DialogNode)NodeGraphUtility.GetConnectedNode(branchingDialogNode.Guid, edges, nodes, portName);
        }

        public void Stop()
        {
            _currentNode?.Stop();
            _currentNode = null;
        }

        public void Finish(string finishBranchId)
        {
            onFinish?.Invoke(finishBranchId);
        }

        private void StartNode(DialogNode dialogNode)
        {
            //_logger.Info($"StartDialogNode {dialogNode}");

            _currentNode?.Stop();
            _currentNode = dialogNode;
            _dialogProfile.NodeId = _currentNode.NodeId;

            // if (dialogNode is BranchingDialogNode)
            // {
            //     _dialogProfile.BranchingId = dialogNode.NodeId;
            // }
            onChangeNode?.Invoke(_dialogProfile);
            _currentNode.Start();
        }

        private void BuildGraph()
        {
            foreach (var node in _nodes)
            {
                node.GraphProcessor = this;
            }

            foreach (var nodeEdge in _edges)
            {
                DialogNode inputNode = null;
                DialogNode outputNode = null;
                foreach (var node in _nodes)
                {
                    if (node.Guid == nodeEdge.InputNodeGuid)
                    {
                        inputNode = node;
                        continue;
                    }
                    if (node.Guid == nodeEdge.OutputNodeGuid)
                    {
                        outputNode = node;
                    }
                }
                try
                {
                    BuildEdge(nodeEdge, inputNode, outputNode);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }

        private void BuildEdge(NodeEdge nodeEdge, DialogNode inputNode, DialogNode outputNode)
        {
            Debug.Assert(inputNode != null, nameof(inputNode) + " != null");
            Debug.Assert(outputNode != null, nameof(outputNode) + " != null");

            var outputProperty = outputNode.GetProperty(nodeEdge.OutputPortName);

            if (outputProperty == null &&
                outputNode is INodeDynamicPorts dynamicPorts &&
                dynamicPorts.DynamicPorts.TryGetFirst(internalDynamicPort => internalDynamicPort.PortName == nodeEdge.OutputPortName, out var dynamicPort))
            {
                outputProperty = dynamicPort.PropertyInfo;
            }

            if (outputProperty == null)
            {
                // _logger.Error($"There isn't found property {nodeEdge.OutputPortName} for nodes {nodeEdge.InputNodeGuid}:{nodeEdge.OutputNodeGuid}");
                return;
            }

            if (outputProperty.PropertyType.IsGenericType &&
                outputProperty.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                var list = (IList)outputProperty.GetValue(outputNode);
                Debug.Assert(list != null, nameof(list) + " != null for type " + outputProperty.PropertyType);

                // TODO Надо как-то переписать получше
                if (outputProperty.PropertyType.GetGenericArguments()[0] == typeof(KeyValueDialogNode))
                {
                    var keyValueGameNode = new KeyValueDialogNode
                    {
                        Key = nodeEdge.OutputPortName,
                        Node = inputNode
                    };
                    list.Add(keyValueGameNode);
                }
                else
                {
                    list.Add(inputNode);
                }
                return;
            }

            if (outputProperty.PropertyType.IsInterface && outputProperty.CanWrite)
            {
                outputProperty.SetValue(outputNode, inputNode);
                return;
            }

            var inputProperty = inputNode.GetProperty(nodeEdge.InputPortName);

            // ReSharper disable once InvertIf
            if (outputProperty.PropertyType == inputProperty.PropertyType)
            {
                if (inputNode.GetType().IsSubclassOf(outputProperty.PropertyType) && outputProperty.CanWrite)
                {
                    outputProperty.SetValue(outputNode, inputNode);
                }
                else
                {
                    inputProperty.SetValue(inputNode, outputNode);
                }
                return;
            }

            throw new NotImplementedException($"There isn't implemented case for propertyType {outputProperty.PropertyType}");
        }
    }
}
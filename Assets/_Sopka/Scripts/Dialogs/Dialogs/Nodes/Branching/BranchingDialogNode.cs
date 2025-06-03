using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    [DialogNode("Branching")]
    public class BranchingDialogNode : DialogNode, INodeDynamicPorts
    {
        [SerializeField]
#if UNITY_EDITOR
        [ValidateInput(nameof(CheckOptions))]
#endif
        private string[] _options = Array.Empty<string>();

        public string[] Options => _options;

#if UNITY_EDITOR

        private bool CheckOptions()
        {
            return _options is { Length: >= 1 } &&
                _options.All(option => _options.Count(option2 => option2 == option) == 1) &&
                _options.All(option => !string.IsNullOrEmpty(option));
        }

#endif

        #region DialogNode

        protected override DialogNode OnCopy()
        {
            return new BranchingDialogNode
            {
                _options = _options
            };
        }

        #endregion

        #region INodeDynamicPorts

        public List<KeyValueDialogNode> NextNodes { get; } = new();

        private static PropertyInfo _nextNodesPropertyInfo;

        IEnumerable<NodeDynamicPort> INodeDynamicPorts.DynamicPorts
        {
            get
            {
                if (_nextNodesPropertyInfo == null)
                {
                    _nextNodesPropertyInfo = typeof(BranchingDialogNode).GetProperty(nameof(NextNodes));
                }
                foreach (var option in _options)
                {
                    if (string.IsNullOrEmpty(option))
                    {
                        continue;
                    }
                    yield return new(NodeGraphUtility.GetPortName(option), NodeDirection.Output, NodePortCapacity.Single, typeof(KeyValueDialogNode), _nextNodesPropertyInfo);
                }
            }
        }

        #endregion
    }
}
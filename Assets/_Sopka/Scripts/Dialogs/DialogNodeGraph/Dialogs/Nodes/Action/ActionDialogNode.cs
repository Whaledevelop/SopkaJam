using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    [DialogNode("Action")]
    public class ActionDialogNode : DialogNode, IOneDirectionNode
    {
        [SerializeReference] private IAction _action;
        
        [SerializeField]
        private string _description;
        
        [NodeProperty("Next", NodeDirection.Output)]
        public DialogNode NextNode { get; set; }

        public string Description => _description;

        protected override DialogNode OnCopy()
        {
            return new ActionDialogNode
            {
                _action = _action
            };
        }

        public UniTask ExecuteAsync(IDiContainer diContainer, CancellationToken cancellationToken = default)
        {
            diContainer.Inject(_action);
            if (_action is IAsyncAction asyncAction)
            {
                return asyncAction.ExecuteAsync(cancellationToken);
            }
            _action.Execute();
            return UniTask.CompletedTask;
        }

        public void Execute(IDiContainer diContainer)
        {
            diContainer.Inject(_action);
            _action.Execute();
        }
    }
}
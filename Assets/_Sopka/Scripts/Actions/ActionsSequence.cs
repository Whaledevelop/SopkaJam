using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ActionsSequence : IAction
    {
        [SerializeReference] private IAction[] _actions;
         
        [Inject] private IDiContainer _diContainer;
        
        public void Execute()
        {
            foreach (var action in _actions)
            {
                _diContainer.Inject(action);
                action.Execute();
            };
        }
    }
}
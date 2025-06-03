using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class AsyncActionsSequence : AsyncAction
    {
        [SerializeReference] private IAsyncAction[] _actions;
        
        private IDiContainer _diContainer;
        
        [Inject]
        private void Construct(IDiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            foreach (var action in _actions)
            {
                _diContainer.Inject(action);
                await action.ExecuteAsync(cancellationToken);
            }
        }
    }
}
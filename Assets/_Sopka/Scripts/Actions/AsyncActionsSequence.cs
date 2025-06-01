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
        
        [Inject] private IDiContainer _diContainer;
        
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            Debug.Log("AsyncActionsSequence");
            foreach (var action in _actions)
            {
                _diContainer.Inject(action);
                await action.ExecuteAsync(cancellationToken);
            }
        }
    }
}
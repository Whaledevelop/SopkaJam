using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class DelayAction : AsyncAction
    {
        [SerializeField] 
        private float _delaySeconds;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.WaitForSeconds(_delaySeconds, cancellationToken: cancellationToken);
        }
    }
}
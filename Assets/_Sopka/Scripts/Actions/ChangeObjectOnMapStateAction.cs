using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeObjectOnMapStateAction : AsyncAction
    {
        [SerializeField] 
        private ObjectOnMapCode _code;

        [SerializeField] private bool _state;

        [Inject] private IGameModel _gameModel;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_state)
            {
                Debug.Log($"Add {_code} to ActiveObjects");
                _gameModel.MapModel.ActiveObjects.Add(_code);
            }
            else
            {
                _gameModel.MapModel.ActiveObjects.Remove(_code);
            }
            return UniTask.CompletedTask;
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class SetGameStateAction : AsyncAction
    {
        [SerializeField]
        private GameState _gameState;
        
        [Inject] private IGameStatesService _gameStatesService;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _gameStatesService.ChangeStateAsync(_gameState, cancellationToken);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.GameStates;

namespace Sopka
{
    [Serializable]
    public class SetGameStateAction : AsyncAction
    {
        [SerializeField]
        private GameState _gameState;
        
        private IGameStatesService _gameStatesService;
        
        [Inject]
        private void Construct(IGameStatesService gameStatesService)
        {
            _gameStatesService = gameStatesService;
        }
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return _gameStatesService.ChangeStateAsync(_gameState, cancellationToken);
        }
    }
}
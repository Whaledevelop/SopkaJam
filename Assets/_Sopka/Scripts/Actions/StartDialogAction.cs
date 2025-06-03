using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.Dialogs;
using Whaledevelop.GameStates;

namespace Sopka
{
    [Serializable]
    public class StartDialogIAsyncAction : AsyncAction
    {
        [SerializeField]
        private DialogSettings _dialogSettings;
        
        [SerializeField]
        private GameState _dialogGameState;

        [SerializeField] 
        private bool _notStartIfAlreadyProcessed = true;
        
        private IGameModel _gameModel;

        private IGameStatesService _gameStatesService;
        
        [Inject]
        private void Construct(IGameModel gameModel, IGameStatesService gameStatesService)
        {
            _gameModel = gameModel;
            _gameStatesService = gameStatesService;
        }
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_notStartIfAlreadyProcessed && _gameModel.DialogModel.ProcessedDialogs.Contains(_dialogSettings))
            {
                return UniTask.CompletedTask;
            }
            _gameModel.DialogModel.PendingDialogSettings = _dialogSettings;
            return _gameStatesService.ChangeStateAsync(_dialogGameState, cancellationToken);
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.Dialogs;

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
        
        [Inject] private IGameModel _gameModel;

        [Inject] private IGameStatesService _gameStatesService;
        
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
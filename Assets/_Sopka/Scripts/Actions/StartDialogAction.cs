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
        
        [Inject] private IGameModel _gameModel;

        [Inject] private IGameStatesService _gameStatesService;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _gameModel.DialogModel.PendingDialogSettings = _dialogSettings;
            return _gameStatesService.ChangeStateAsync(_dialogGameState, cancellationToken);
        }
    }
}
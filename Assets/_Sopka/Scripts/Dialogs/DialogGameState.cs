using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.Dialogs;
using Whaledevelop.GameStates;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/DialogGameState", fileName = "DialogGameState")]
    public class DialogGameState : GameState
    {
        [Inject] private IGameModel _gameModel;

        [Inject] private IDialogsService _dialogsService;

        [Inject] private IDiContainer _diContainer;
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            ProcessCurrentDialogAsync(cancellationToken).Forget();
            return UniTask.CompletedTask;
        }

        private async UniTask ProcessCurrentDialogAsync(CancellationToken cancellationToken)
        {
            var dialogSettings = _gameModel.DialogModel.PendingDialogSettings;
            if (dialogSettings == null)
            {
                Debug.LogError("No CurrentDialogSettings");
                return;
            }
            _gameModel.DialogModel.ProcessingDialog.Value = dialogSettings;
            await _dialogsService.ExecuteDialogAsync(dialogSettings, cancellationToken);
            _gameModel.DialogModel.ProcessingDialog.Value = null;
            _gameModel.DialogModel.ProcessedDialogs.Add(dialogSettings);
            _gameModel.DialogModel.PendingDialogSettings = null;

            if (dialogSettings.AfterDialogAction != null)
            {
                _diContainer.Inject(dialogSettings.AfterDialogAction);
                dialogSettings.AfterDialogAction.Execute();
            }
        }

        public override UniTask EnableAsync(CancellationToken cancellationToken)
        {
            ProcessCurrentDialogAsync(cancellationToken).Forget();
            return UniTask.CompletedTask;
        }

        public override UniTask DisableAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}
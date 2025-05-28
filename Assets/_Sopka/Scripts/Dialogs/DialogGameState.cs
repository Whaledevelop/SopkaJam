using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.Dialogs;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/DialogGameState", fileName = "DialogGameState")]
    public class DialogGameState : GameState
    {
        [SerializeReference] private IAction _action;
        
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
            if (_gameModel.DialogModel.CurrentDialogSettings == null)
            {
                Debug.LogError("No CurrentDialogSettings");
                return;
            }
            await _dialogsService.ExecuteDialogAsync(_gameModel.DialogModel.CurrentDialogSettings, cancellationToken);
            _gameModel.DialogModel.CurrentDialogSettings = null;
            _diContainer.Inject(_action);
            _action.Execute();
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
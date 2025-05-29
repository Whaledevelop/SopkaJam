using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.Dialogs;

namespace Sopka
{
    [Serializable]
    public class WaitUntilDialogProcessed : AsyncAction
    {
        [SerializeField] private DialogSettings _dialogSettings;

        [Inject] private IGameModel _gameModel;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return UniTask.WaitUntil(() => _gameModel.DialogModel.ProcessedDialogs.Contains(_dialogSettings), cancellationToken:cancellationToken);
        }
    }
}
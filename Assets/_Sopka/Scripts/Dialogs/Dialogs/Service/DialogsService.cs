using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.Services;

namespace Whaledevelop.Dialogs
{
    [CreateAssetMenu(fileName = "DialogsService", menuName = "Whaledevelop/Services/DialogsService")]
    public class DialogsService : Service, IDialogsService
    {
        [SerializeField]
        private DialogUISettings _dialogUISettings;

        private IDiContainer _diContainer;
        
        [Inject]
        private void Construct(IDiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        public async UniTask ExecuteDialogAsync(IDialogSettings dialogSettings, CancellationToken cancellationToken)
        {
            var isDialogFinished = false;
            DialogNodeGraphProcessor dialogGraphProcessor = null;
            DialogProcessor dialogProcessor = null;

            try
            {
                dialogGraphProcessor = new DialogNodeGraphProcessor(dialogSettings, dialogSettings.Id);
                dialogGraphProcessor.onChangeNode += OnChangeDialogNode;
                dialogGraphProcessor.onFinish += OnNodeGraphFinish;

                dialogProcessor = new DialogProcessor(dialogSettings, _dialogUISettings);
                _diContainer.Inject(dialogProcessor);

                dialogProcessor.OnEndNode += dialogGraphProcessor.EndDialogNode;
                dialogProcessor.OnChooseOption += dialogGraphProcessor.ChooseDialogOption;
                dialogProcessor.OnEndDialog += OnProcessorEnd;

                dialogGraphProcessor.Start();

                await UniTask.WaitUntil(() => isDialogFinished, cancellationToken: cancellationToken);
            }
            finally
            {
                if (dialogGraphProcessor != null)
                {
                    dialogGraphProcessor.onChangeNode -= OnChangeDialogNode;
                    dialogGraphProcessor.onFinish -= OnNodeGraphFinish;

                    if (dialogProcessor != null)
                    {
                        dialogProcessor.OnEndNode -= dialogGraphProcessor.EndDialogNode;
                        dialogProcessor.OnChooseOption -= dialogGraphProcessor.ChooseDialogOption;
                        dialogProcessor.OnEndDialog -= OnProcessorEnd;
                        dialogProcessor.Dispose();
                    }
                }
            }
            return;

            void OnProcessorEnd()
            {
                isDialogFinished = true;
            }

            void OnNodeGraphFinish(string branchId)
            {
                isDialogFinished = true;
            }

            void OnChangeDialogNode(DialogProfile dialogProfile)
            {
                // ReSharper disable once AccessToModifiedClosure
                // ReSharper disable once AccessToDisposedClosure
                dialogProcessor?.ProcessDialogNode(dialogProfile);
            }
        }
    }
}
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whaledevelop;
using Whaledevelop.DiContainer;

namespace Sopka
{
    [Serializable]
    public class RestartAction : AsyncAction
    {
        [SerializeField] private bool _showLoading = true;

        [SerializeField] private string _loadingText = "Вы выжили в голодной экспедиции и прошли игру!";
        
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (DiContainerUtility.MainContainer.TryResolve<IServicesContainer>(out var services))
            {
                await services.ReleaseAsync(cancellationToken);
            }

            if (_showLoading)
            {
                LoadingUIController.Show(_loadingText);
            }


            ProjectContext.Reset();

            GC.Collect();

            await SceneManager.LoadSceneAsync(0).ToUniTask(cancellationToken: cancellationToken);

            if (_showLoading)
            {
                LoadingUIController.Hide();
            }
        }
    }
}
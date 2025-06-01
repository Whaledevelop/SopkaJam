using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Whaledevelop;
using Whaledevelop.DiContainer;

namespace Sopka
{
    [Serializable]
    public class RestartAction : AsyncAction
    {
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (DiContainerUtility.MainContainer.TryResolve<IServicesContainer>(out var services))
            {
                await services.ReleaseAsync(cancellationToken);
            }

            ProjectContext.Reset();

            GC.Collect();

            await SceneManager.LoadSceneAsync(0).ToUniTask(cancellationToken: cancellationToken);
        }
    }
}
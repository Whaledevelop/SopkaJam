using System.Threading;
using Cysharp.Threading.Tasks;
using Whaledevelop.Services;

namespace Whaledevelop.Dialogs
{
    public interface IDialogsService : IService
    {
        UniTask ExecuteDialogAsync(IDialogSettings dialogSettings, CancellationToken cancellationToken);
    }
}
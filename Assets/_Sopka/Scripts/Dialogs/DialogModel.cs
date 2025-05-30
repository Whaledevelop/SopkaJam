using Whaledevelop.Dialogs;
using Whaledevelop.Reactive;

namespace Sopka
{
    public class DialogModel
    {
        public ReactiveCollection<IDialogSettings> ProcessedDialogs = new();
        public IDialogSettings PendingDialogSettings { get; set; }
    }
}
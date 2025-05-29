using System;
using Whaledevelop.UI;

namespace Sopka
{
    public class StartMenuViewModel : IUIViewModel
    {
        public Action OnClickStart;

        public Action OnClickQuit;

        public StartMenuViewModel(Action onClickStart, Action onClickQuit)
        {
            OnClickStart = onClickStart;
            OnClickQuit = onClickQuit;
        }
    }
}
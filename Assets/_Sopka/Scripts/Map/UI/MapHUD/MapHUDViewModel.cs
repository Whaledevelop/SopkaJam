using System;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Sopka
{
    public class MapHUDViewModel : IUIViewModel
    {
        public MapHUDViewModel(ReactiveValue<int> teamMembersCount, ReactiveValue<int> suppliesCount, ReactiveValue<float> hungerProgress, Action onClickExit)
        {
            TeamMembersCount = teamMembersCount;
            SuppliesCount = suppliesCount;
            HungerProgress = hungerProgress;
            OnClickExit = onClickExit;
        }

        public ReactiveValue<int> TeamMembersCount { get; }
        
        public ReactiveValue<int> SuppliesCount { get;  } 
        
        public ReactiveValue<float> HungerProgress { get;  }
        
        public Action OnClickExit { get; }
    }
}
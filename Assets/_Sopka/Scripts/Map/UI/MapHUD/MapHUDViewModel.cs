using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Sopka
{
    public class MapHUDViewModel : IUIViewModel
    {
        public MapHUDViewModel(ReactiveValue<int> teamMembersCount, ReactiveValue<int> suppliesCount, ReactiveValue<float> hungerProgress)
        {
            TeamMembersCount = teamMembersCount;
            SuppliesCount = suppliesCount;
            HungerProgress = hungerProgress;
        }

        public ReactiveValue<int> TeamMembersCount { get; }
        
        public ReactiveValue<int> SuppliesCount { get;  } 
        
        public ReactiveValue<float> HungerProgress { get;  }
    }
}
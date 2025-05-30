using Whaledevelop.Reactive;

namespace Sopka
{
    public class ResourcesModel
    {
        public ResourcesModel(ResourceModelStartSettings modelStartSettings)
        {
            TeamMembersCount = new ReactiveValue<int>(modelStartSettings.TeamMembers);
            SuppliesCount = new ReactiveValue<int>(modelStartSettings.Supplies);
            HungerProgress = new ReactiveValue<float>(modelStartSettings.HungerProgress);
            HungerActive = new ReactiveValue<bool>(modelStartSettings.HungerActive);
            HungerDefaultSpeed = new ReactiveValue<float>(modelStartSettings.HungerDefaultSpeed);
        }

        public ReactiveValue<int> TeamMembersCount { get; }

        public ReactiveValue<int> SuppliesCount { get; }

        public ReactiveValue<float> HungerProgress { get; }

        public ReactiveValue<bool> HungerActive { get; }

        public ReactiveValue<float> HungerDefaultSpeed { get; }
    }
}
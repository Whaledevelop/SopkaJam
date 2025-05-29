using Whaledevelop.Reactive;

namespace Sopka
{
    public class ResourcesModel
    {
        public ResourcesModel(ResourceStartValues startValues)
        {
            TeamMembersCount = new ReactiveValue<int>(startValues.TeamMembers);
            SuppliesCount = new ReactiveValue<int>(startValues.Supplies);
            HungerProgress = new ReactiveValue<float>(startValues.HungerProgress);
            HungerActive = new ReactiveValue<bool>(startValues.HungerActive);
            HungerDefaultSpeed = new ReactiveValue<float>(startValues.HungerDefaultSpeed);
        }

        public ReactiveValue<int> TeamMembersCount { get; }

        public ReactiveValue<int> SuppliesCount { get; }

        public ReactiveValue<float> HungerProgress { get; }

        public ReactiveValue<bool> HungerActive { get; }

        public ReactiveValue<float> HungerDefaultSpeed { get; }
    }
}
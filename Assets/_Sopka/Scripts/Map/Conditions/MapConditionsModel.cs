using Whaledevelop.Reactive;

namespace Sopka
{
    public class MapConditionsModel
    {
        public readonly ReactiveCollection<ConditionCode> FulfilledConditions = new ();
    }
}
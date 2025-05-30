using JetBrains.Annotations;
using Whaledevelop.Reactive;

namespace Sopka
{
    public class MapModel
    {
        public MapView MapView;

        public MapPlayerView MapPlayerView;

        public ReactiveValue<MapLocationCode> CurrentLocation;
        
        [CanBeNull]
        public MapPath CurrentPath;

        public MapPlayerState MapPlayerState;
        

        public ReactiveCollection<MapLocationCode> OpenedLocations;
        public ReactiveCollection<MapPathCode> OpenedPaths;
        public ReactiveCollection<ObjectOnMapCode> ActiveObjects;

        public readonly ReactiveCollection<ConditionCode> FulfilledConditions = new ();
        

        public ReactiveValue<float> MapMovementSpeed = new();

        // TODO перенести в стартовые настройки
        public float DefaultMovementSpeed;
    }
}
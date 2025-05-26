using JetBrains.Annotations;
using Whaledevelop.Reactive;

namespace Sopka
{
    public enum MapPlayerState
    {
        Static,
        Moving
    }
    
    public class MapModel
    {
        public MapView MapView;

        public MapPlayerView MapPlayerView;

        public ReactiveValue<MapLocationCode> CurrentLocation;
        
        [CanBeNull]
        public MapPath CurrentPath;

        public MapPlayerState MapPlayerState;
    }
}
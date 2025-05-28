using System.Collections.Generic;
using UnityEngine;
using Whaledevelop.Utility;

namespace Sopka
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<MapLocationCode, MapLocationView> _mapLocationViews;
        
        [SerializeField] private MapPath[] _mapPaths;
        
        [SerializeField] private SerializableDictionary<ObjectOnMapCode, ObjectOnMapView> _objectOnMapViews;

        public IReadOnlyDictionary<MapLocationCode, MapLocationView> MapLocationViews => _mapLocationViews;

        public IEnumerable<MapPath> MapPaths => _mapPaths;

        public IReadOnlyDictionary<ObjectOnMapCode, ObjectOnMapView> ObjectOnMapViews =>
            _objectOnMapViews;
    }
}
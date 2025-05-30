using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop.Utility;

namespace Sopka
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<MapLocationCode, MapLocationView> _mapLocationViews;
        
        [SerializeField] private SerializableDictionary<MapPathCode, MapPath> _mapPathsViews;
        
        [SerializeField] private SerializableDictionary<ObjectOnMapCode, ObjectOnMapView> _objectOnMapViews;

        [SerializeField] private SerializableDictionary<MapPathCode, PathGnusView> _pathGnusViews;

        public IReadOnlyDictionary<MapLocationCode, MapLocationView> MapLocationViews => _mapLocationViews;

        public IReadOnlyDictionary<MapPathCode, MapPath> MapPathsViews => _mapPathsViews;

        public IReadOnlyDictionary<ObjectOnMapCode, ObjectOnMapView> ObjectOnMapViews =>
            _objectOnMapViews;

        public IReadOnlyDictionary<MapPathCode, PathGnusView> PathGnusViews => _pathGnusViews;
    }
}
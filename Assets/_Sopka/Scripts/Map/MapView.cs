using System.Collections.Generic;
using UnityEngine;
using Whaledevelop.Utility;

namespace Sopka
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<MapLocationCode, MapLocationView> _mapLocationViews;
        
        [SerializeField] private MapPath[] _mapPaths;

        public IReadOnlyDictionary<MapLocationCode, MapLocationView> MapLocationViews => _mapLocationViews;

        public IEnumerable<MapPath> MapPaths => _mapPaths;
    }
}
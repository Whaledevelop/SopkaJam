using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/MapMovementSystem", fileName = "MapMovementSystem")]
    public class MapMovementSystem : GameSystem
    {
        [SerializeField]
        private float _speed = 3f;

        [SerializeField]
        private PathType _pathType = PathType.CatmullRom;

        [SerializeField]
        private Ease _ease = Ease.InOutSine;

        [SerializeField]
        private bool _lookForward = true;
        
        private Tweener _tween;

        [Inject] private IGameModel _gameModel;
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            foreach (var (locationCode, locationView) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                locationView.OnMouseDownEvent = () => OnMapLocationClicked(locationCode);
            }
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            foreach (var (locationCode, locationView) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                locationView.OnMouseDownEvent = null;
            }
            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnMapLocationClicked(MapLocationCode nextLocation)
        {
            if (!TryGetPathToLocation(nextLocation, out var mapPath))
            {
                Debug.Log("Not path to location");
                return;
            }
            if (!_gameModel.MapModel.OpenedPaths.Contains(mapPath.PathCode))
            {
                Debug.Log("Not opened map path");
                return;
            }
            MoveAlongPath(mapPath);
        }

        private bool TryGetPathToLocation(MapLocationCode location, out MapPath mapPath)
        {
            var mapModel = _gameModel.MapModel;
            mapPath = null;
            if (mapModel.MapPlayerState == MapPlayerState.Moving || mapModel.CurrentLocation == null) 
            {
                Debug.Log("Already moving or location not set");
                return false;
            }
            var currentLocation = mapModel.CurrentLocation.Value;
            mapPath = mapModel.MapView.MapPaths.FirstOrDefault(path =>
                (path.LocationA == location && path.LocationB == currentLocation)
                || path.LocationA == currentLocation && path.LocationB == location);
            if (mapPath == null)
            {
                Debug.LogError($"No path from {currentLocation} to {location}");
                return false;
            }
            return true;
        }
        
        // protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        // {
        //     return base.OnReleaseAsync(cancellationToken);
        // }
        
        private void MoveAlongPath(MapPath mapPath)
        {
            var mapModel = _gameModel.MapModel;
            var mapView = mapModel.MapView;
            var playerTransform = mapModel.MapPlayerView.transform;

            var isReversed = mapPath.LocationB == mapModel.CurrentLocation.Value;
            var destination = isReversed ? mapPath.LocationA : mapPath.LocationB;
            var waypoints = isReversed
                ? mapPath.Waypoints.Reverse().ToArray()
                : mapPath.Waypoints;
            
            if (!mapView.MapLocationViews.TryGetValue(mapModel.CurrentLocation.Value, out var startLocationView) ||
                !mapView.MapLocationViews.TryGetValue(destination, out var endLocationView))
            {
                Debug.LogError("Start or end location not found");
                return;
            }

            var startPos = startLocationView.PlayerRoot.position;
            if (Vector3.Distance(playerTransform.position, startPos) > 0.05f)
            {
                Debug.LogError($"Player is not at start! Player: {playerTransform.position}, Expected: {startPos}");
                return;
            }
            var endPos = endLocationView.PlayerRoot.position;
            
            var path = new List<Vector3>();
            for (var i = 0; i < waypoints.Length; i++)
            {
                var pos = waypoints[i].position;
                if (i == 0)
                {
                    if (Vector3.Distance(pos, startPos) > 0.05f)
                    {
                        Debug.LogError($"Error with start point");
                        return;
                    }
                    path.Add(pos);
                    continue;
                }

                if (i == waypoints.Length - 1 && Vector3.Distance(pos, endPos) > 0.05f)
                {
                    Debug.LogError($"Error with end point");
                    return;
                    
                }
                path.Add(pos);
            }

            //Debug.Log($"[Path] From {startPos} → {endPos}, Waypoints: {waypoints.Length}, Final Count: {path.Count}");
            
            var totalDistance = 0f;
            for (var i = 0; i < path.Count - 1; i++)
            {
                totalDistance += Vector3.Distance(path[i], path[i + 1]);
            }

            var duration = totalDistance / _speed;

            mapModel.MapPlayerState = MapPlayerState.Moving;

            _tween?.Kill();

            _tween = playerTransform
                .DOPath(path.ToArray(), duration, _pathType, PathMode.Full3D)
                .SetOptions(_lookForward)
                .SetEase(_ease)
                .OnComplete(() =>
                {
                    Debug.Log($"== Arrived at {destination} ({endPos}) ==");
                    mapModel.CurrentLocation.Value = destination;
                    mapModel.MapPlayerState = MapPlayerState.Static;
                });
        }


    }
}
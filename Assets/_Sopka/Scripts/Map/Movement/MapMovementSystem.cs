using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/MapMovementSystem", fileName = "MapMovementSystem")]
    public class MapMovementSystem : GameSystem
    {
        [SerializeField]
        private float _defaultSpeed = 2.5f;

        [SerializeField]
        private PathType _pathType = PathType.CatmullRom;

        [SerializeField]
        private Ease _ease = Ease.InOutSine;

        // [SerializeField]
        // private bool _lookForward = true;

        private Tweener _tween;

        private float _baseSpeed;
        private float _baseDuration;
        private float _currentTweenDistance;

        [Inject] private IGameModel _gameModel;

        private readonly List<IDisposable> _subscriptions = new();

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            foreach (var (locationCode, locationView) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                locationView.OnMouseDownEvent = () => OnMapLocationClicked(locationCode);
            }

            _gameModel.MapModel.MapMovementSpeed = new ReactiveValue<float>(_defaultSpeed);
            _gameModel.MapModel.DefaultMovementSpeed = _defaultSpeed;
            _gameModel.MapModel.MapMovementSpeed.Subscribe(OnChangeSpeed).AddToCollection(_subscriptions);

            return base.OnInitializeAsync(cancellationToken);
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            foreach (var (locationCode, locationView) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                locationView.OnMouseDownEvent = null;
            }

            _subscriptions.Dispose();

            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnChangeSpeed(float speed)
        {
            if (_tween == null || !_tween.IsPlaying())
            {
                return;
            }

            if (Mathf.Approximately(_baseSpeed, 0f))
            {
                return;
            }

            var newTimeScale = speed / _baseSpeed;

            _tween.timeScale = newTimeScale;
        }

        private void OnMapLocationClicked(MapLocationCode nextLocation)
        {
            if (!TryGetPathToLocation(nextLocation, out var mapPath, out var pathCode))
            {
                Debug.Log("Not path to location");
                return;
            }

            if (!_gameModel.MapModel.OpenedPaths.Contains(pathCode))
            {
                Debug.Log("Not opened map path");
                return;
            }

            MoveAlongPath(mapPath);
        }

        private bool TryGetPathToLocation(MapLocationCode location, out MapPath mapPath, out MapPathCode pathCode)
        {
            var mapModel = _gameModel.MapModel;
            mapPath = null;
            pathCode = default;
            if (mapModel.MapPlayerState.Value == MapPlayerState.Moving || mapModel.CurrentLocation == null) 
            {
                Debug.Log("Already moving or location not set");
                return false;
            }

            var currentLocation = mapModel.CurrentLocation.Value;

            mapPath = mapModel.MapView.MapPathsViews.FirstOrDefault(kvp =>
                (kvp.Value.LocationA == location && kvp.Value.LocationB == currentLocation)
                || kvp.Value.LocationA == currentLocation && kvp.Value.LocationB == location).Value;

            if (mapPath == null)
            {
                Debug.LogError($"No path from {currentLocation} to {location}");
                return false;
            }

            return true;
        }

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

            _currentTweenDistance = 0f;

            for (var i = 0; i < path.Count - 1; i++)
            {
                _currentTweenDistance += Vector3.Distance(path[i], path[i + 1]);
            }

            _baseSpeed = _gameModel.MapModel.MapMovementSpeed.Value;
            _baseDuration = _currentTweenDistance / _baseSpeed;

            mapModel.MapPlayerState.Value = MapPlayerState.Moving;

            _tween?.Kill();

            
            _tween = playerTransform
                .DOPath(path.ToArray(), _baseDuration, _pathType)
                .SetEase(_ease)
                .OnComplete(() =>
                {
                    mapModel.CurrentLocation.Value = destination;
                    mapModel.MapPlayerState.Value = MapPlayerState.Static;

                });
        }
    }
}

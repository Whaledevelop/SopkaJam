using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/MapGameState", fileName = "MapGameState")]
    public class MapGameState : GameState
    {
        [SerializeField] private MapView _mapViewPrefab;

        [SerializeField] private MapPlayerView _mapPlayerViewPrefab;

        [SerializeField] private GameSystem[] _systems;

        [SerializeField] private MapLocationCode _startLocation;
        
        [Inject] private IGameModel _gameModel;
        
        [Inject] private IGameSystemsService _gameSystemsService;
        
        protected override async UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            var mapView = _gameModel.MapModel.MapView = Instantiate(_mapViewPrefab);

            if (!mapView.MapLocationViews.TryGetValue(_startLocation, out var mapLocationView))
            {
                Debug.LogError($"Map Location View {_startLocation} not found");
                return;
            }
            _gameModel.MapModel.MapPlayerView = Instantiate(_mapPlayerViewPrefab, mapLocationView.PlayerRoot);

            _gameModel.MapModel.CurrentLocation = new ReactiveValue<MapLocationCode>(_startLocation);

            foreach (var system in _systems)
            {
                await _gameSystemsService.AddSystemAsync(system, cancellationToken);
            }

        }

        protected override async UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            Destroy(_gameModel.MapModel.MapView.gameObject);
            Destroy(_gameModel.MapModel.MapPlayerView.gameObject);
            _gameModel.MapModel.MapView = null;
            _gameModel.MapModel.MapPlayerView = null;
            foreach (var system in _systems)
            {
                await _gameSystemsService.RemoveSystemAsync(system, cancellationToken);
            }
        }
    }
}
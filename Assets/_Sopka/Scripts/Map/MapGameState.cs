using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/MapGameState", fileName = "MapGameState")]
    public class MapGameState : GameState
    {
        [SerializeField] private MapView _mapViewPrefab;

        [SerializeField] private MapPlayerView _mapPlayerViewPrefab;

        [SerializeField] private MapLocationCode _startLocation;

        [SerializeField] private MapLocationCode[] _openedLocationsFromStart;
        
        [SerializeField] private MapPathCode[] _openedPathsFromStart;

        [SerializeReference] private IAction _initialAction;
        
        [Inject] private IGameModel _gameModel;

        [Inject] private IDiContainer _diContainer;
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            var mapView = _gameModel.MapModel.MapView = Instantiate(_mapViewPrefab);

            if (!mapView.MapLocationViews.TryGetValue(_startLocation, out var mapLocationView))
            {
                Debug.LogError($"Map Location View {_startLocation} not found");
                return UniTask.CompletedTask;
            }
            _gameModel.MapModel.MapPlayerView = Instantiate(_mapPlayerViewPrefab, mapLocationView.PlayerRoot);

            _gameModel.MapModel.CurrentLocation = new ReactiveValue<MapLocationCode>(_startLocation);

            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = _openedLocationsFromStart.Contains(code);
            }
            _gameModel.MapModel.OpenedLocations = new ReactiveCollection<MapLocationCode>(_openedLocationsFromStart);
            
            foreach (var mapPath in _gameModel.MapModel.MapView.MapPaths)
            {
                mapPath.LineRenderer.enabled = _openedPathsFromStart.Contains(mapPath.PathCode);
            }
            _gameModel.MapModel.OpenedPaths = new ReactiveCollection<MapPathCode>(_openedPathsFromStart);

            _gameModel.MapModel.ActiveObjects = new ReactiveCollection<ObjectOnMapCode>();
            
            Debug.Log($"Initialize Map");
            
            _diContainer.Inject(_initialAction);
            _initialAction.Execute();
            
            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _gameModel.MapModel.OpenedLocations.Clear();
            _gameModel.MapModel.OpenedPaths.Clear();
            _gameModel.MapModel.CurrentLocation = null;
            
            Destroy(_gameModel.MapModel.MapView.gameObject);
            Destroy(_gameModel.MapModel.MapPlayerView.gameObject);
            _gameModel.MapModel.MapView = null;
            _gameModel.MapModel.MapPlayerView = null;
            return UniTask.CompletedTask;
        }

        public override UniTask EnableAsync(CancellationToken cancellationToken)
        {

            _gameModel.MapModel.MapView.gameObject.SetActive(true);
            _gameModel.MapModel.MapPlayerView.gameObject.SetActive(true);
            var openedLocations = _gameModel.MapModel.OpenedLocations;
            
            Debug.Log($"Enable Map {openedLocations.Count}");
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = openedLocations.Contains(code);
            }
            var openedPaths = _gameModel.MapModel.OpenedPaths;
            foreach (var mapPath in _gameModel.MapModel.MapView.MapPaths)
            {
                mapPath.LineRenderer.enabled = openedPaths.Contains(mapPath.PathCode);
            }
            return UniTask.CompletedTask;
        }

        public override UniTask DisableAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Disable Map");
            _gameModel.MapModel.MapView.gameObject.SetActive(false);
            _gameModel.MapModel.MapPlayerView.gameObject.SetActive(false);
            
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = false;
            }
            foreach (var mapPath in _gameModel.MapModel.MapView.MapPaths)
            {
                mapPath.LineRenderer.enabled = false;
            }
            return UniTask.CompletedTask;
        }
    }
}
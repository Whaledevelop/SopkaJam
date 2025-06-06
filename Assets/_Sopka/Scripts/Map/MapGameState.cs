﻿using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.GameStates;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

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

        [SerializeField] private MapHUDView _mapHUDViewPrefab;
        
        private IGameModel _gameModel;

        private IDiContainer _diContainer;

        private IUIService _uiService;
        
        private MapHUDViewModel _mapHUDViewModel;
        
        [Inject]
        private void Construct(IGameModel gameModel, IUIService uiService, IDiContainer diContainer)
        {
            _gameModel = gameModel;
            _uiService = uiService;
            _diContainer = diContainer;
        }
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            var mapView = _gameModel.MapModel.MapView = Instantiate(_mapViewPrefab);

            if (!mapView.MapLocationViews.TryGetValue(_startLocation, out var mapLocationView))
            {
                Debug.LogError($"Map Location View {_startLocation} not found");
                return UniTask.CompletedTask;
            }
            var mapPlayerView = _gameModel.MapModel.MapPlayerView = Instantiate(_mapPlayerViewPrefab, mapView.transform);

            mapPlayerView.transform.position = mapLocationView.transform.position;
            mapPlayerView.transform.rotation = mapLocationView.transform.rotation;
            
            _gameModel.MapModel.CurrentLocation = new ReactiveValue<MapLocationCode>(_startLocation);

            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = _openedLocationsFromStart.Contains(code);
            }
            _gameModel.MapModel.OpenedLocations = new ReactiveCollection<MapLocationCode>(_openedLocationsFromStart);
            
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapPathsViews)
            {
                view.LineRenderer.enabled = _openedPathsFromStart.Contains(code);
            }
            _gameModel.MapModel.OpenedPaths = new ReactiveCollection<MapPathCode>(_openedPathsFromStart);

            _gameModel.MapModel.ActiveObjects = new ReactiveCollection<ObjectOnMapCode>();
            
            _diContainer.Inject(_initialAction);
            _initialAction.Execute();
            OpenHUD();
            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            if (_gameModel.MapModel != null)
            {
                _gameModel.MapModel.OpenedLocations.Clear();
                _gameModel.MapModel.OpenedPaths.Clear();
                _gameModel.MapModel.CurrentLocation = null;

                if (_gameModel.MapModel.MapView != null)
                {                
                    Destroy(_gameModel.MapModel.MapView.gameObject);
                    _gameModel.MapModel.MapView = null;
                }

                if (_gameModel.MapModel.MapPlayerView != null)
                {
                    Destroy(_gameModel.MapModel.MapPlayerView.gameObject);
 
                    _gameModel.MapModel.MapPlayerView = null;
                }
                
            }

            CloseHUD();
            return UniTask.CompletedTask;
        }

        public override UniTask EnableAsync(CancellationToken cancellationToken)
        {

            _gameModel.MapModel.MapView.gameObject.SetActive(true);
            _gameModel.MapModel.MapPlayerView.gameObject.SetActive(true);
            var openedLocations = _gameModel.MapModel.OpenedLocations;
            
            // Debug.Log($"Enable Map {openedLocations.Count}");
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = openedLocations.Contains(code);
            }
            var openedPaths = _gameModel.MapModel.OpenedPaths;
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapPathsViews)
            {
                view.LineRenderer.enabled = openedPaths.Contains(code);
            }

            OpenHUD();
            
            return UniTask.CompletedTask;
        }

        private void OpenHUD()
        {
            CloseHUD();
            var resourcesModel = _gameModel.ResourcesModel;
            _mapHUDViewModel = new MapHUDViewModel(resourcesModel.TeamMembersCount, resourcesModel.SuppliesCount, resourcesModel.HungerProgress, OnClickExit);
            _uiService.OpenView(_mapHUDViewPrefab, _mapHUDViewModel);
        }

        private void OnClickExit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void CloseHUD()
        {
            if (_mapHUDViewModel != null)
            {
                _uiService.CloseView(_mapHUDViewModel);
            }
        }

        public override UniTask DisableAsync(CancellationToken cancellationToken)
        {
            if (_gameModel.MapModel != null)
            {
                if (_gameModel.MapModel.MapView != null)
                {
                    _gameModel.MapModel.MapView.gameObject.SetActive(false);
                    foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
                    {
                        view.Renderer.enabled = false;
                    }
                    foreach (var (code, view) in _gameModel.MapModel.MapView.MapPathsViews)
                    {
                        view.LineRenderer.enabled = false;
                    }
                }

                if (_gameModel.MapModel.MapPlayerView != null)
                {
                    _gameModel.MapModel.MapPlayerView.gameObject.SetActive(false);
                }
            }


            CloseHUD();
            return UniTask.CompletedTask;
        }
    }
}
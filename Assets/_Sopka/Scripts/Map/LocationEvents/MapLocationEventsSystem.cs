﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.GameSystems;
using Whaledevelop.Reactive;
using Whaledevelop.Utility;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/MapLocationEventsSystem", fileName = "MapLocationEventsSystem")]
    public class MapLocationEventsSystem : GameSystem
    {
        [SerializeReference]
        private LocationEventData[] _locationsEventsData;
        
        // [SerializeField]

        private IGameModel _gameModel;

        private IDiContainer _diContainer;

        private List<IDisposable> _subscriptions = new();
        
        [Inject]
        private void Construct(IGameModel gameModel, IDiContainer diContainer)
        {
            _gameModel = gameModel;
            _diContainer = diContainer;
        }
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _subscriptions = new List<IDisposable>();
            
            _gameModel.MapModel.CurrentLocation.Subscribe(OnLocationChanged).AddToCollection(_subscriptions);

            _gameModel.MapModel.OpenedPaths.SubscribeChanged(OnPathsChanged).AddToCollection(_subscriptions);
            
            _gameModel.MapModel.OpenedLocations.SubscribeChanged(OnLocationsChanged).AddToCollection(_subscriptions);
            
            return base.OnInitializeAsync(cancellationToken);
        }

        private void OnPathsChanged()
        {
            var openedPaths = _gameModel.MapModel.OpenedPaths;
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapPathsViews)
            {
                view.LineRenderer.enabled = openedPaths.Contains(code);
            }
        }

        private void OnLocationsChanged()
        {
            var openedLocations = _gameModel.MapModel.OpenedLocations;
            foreach (var (code, view) in _gameModel.MapModel.MapView.MapLocationViews)
            {
                view.Renderer.enabled = openedLocations.Contains(code);
            }
        }
        

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _subscriptions.Dispose();
            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnLocationChanged(MapLocationCode locationCode)
        {
            var locationEventDatas = _locationsEventsData.Where(data => data.LocationCode == locationCode).ToArray();
            if (!locationEventDatas.Any())
            {
                return;
            }
            var fulfilledConditions = _gameModel.MapModel.FulfilledConditions.ToArray();
            var locationEventData = fulfilledConditions.Length == 0
                ? locationEventDatas[0]
                : locationEventDatas.FirstOrDefault(data => data.RequiredConditions.All(condition => fulfilledConditions.Contains(condition)));
            if (locationEventData == null)
            {
                Debug.Log($"No action for location {locationCode} with all fullfilled conditions");
                return;
            }
            ExecuteLocationActionAsync(locationEventData).Forget();
        }
 
        private async UniTask ExecuteLocationActionAsync(LocationEventData locationEventData)
        {
            if (locationEventData.TimeoutBeforeAction > 0)
            {
                await UniTask.WaitForSeconds(locationEventData.TimeoutBeforeAction);
            }
            _diContainer.Inject(locationEventData.Action);
            locationEventData.Action.Execute();
        }
    }
}
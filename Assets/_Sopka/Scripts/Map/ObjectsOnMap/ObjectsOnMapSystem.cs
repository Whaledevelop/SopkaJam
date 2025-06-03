using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.GameSystems;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/ObjectsOnMapSystem", fileName = "ObjectsOnMapSystem")]
    public class ObjectsOnMapSystem : GameSystem
    {
        [SerializeField]
        private ObjectOnMapEventData[] _objectOnMapEventsData;
        
        [Inject] private IGameModel _gameModel;

        [Inject] private IDiContainer _diContainer;
        
        private readonly List<IDisposable> _subscriptions = new();
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _gameModel.MapModel.ActiveObjects.SubscribeChanged(UpdateActiveObjects).AddToCollection(_subscriptions);
            // foreach (var (code, view) in _gameModel.MapModel.MapView.ObjectOnMapViews)
            // {
            //     view.OnMouseDownEvent = () => ObjectOnMapClick(code);
            // }

            UpdateActiveObjects();
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _subscriptions.Dispose();
            // foreach (var (code, view) in _gameModel.MapModel.MapView.ObjectOnMapViews)
            // {
            //     view.OnMouseDownEvent = null;
            // }
            return base.OnReleaseAsync(cancellationToken);
        }

        private void UpdateActiveObjects()
        {
            var activeObjects = _gameModel.MapModel.ActiveObjects;
            foreach (var (code, view) in _gameModel.MapModel.MapView.ObjectOnMapViews)
            {
                view.SetMode(activeObjects.Contains(code));
            }
        }

        // private void ObjectOnMapClick(ObjectOnMapCode code)
        // {
        //     var eventData = _objectOnMapEventsData.FirstOrDefault(eventData => eventData.Code == code);
        //     if (eventData == null)
        //     {
        //         Debug.Log($"No action for object {code}");
        //         return;
        //     }
        //     _diContainer.Inject(eventData.Action);
        //     eventData.Action.Execute();
        // }
    }
}
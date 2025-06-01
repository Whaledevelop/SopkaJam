using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Whaledevelop;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/GnusSystem", fileName = "GnusSystem")]
    public class GnusSystem : GameSystem, IUpdate
    {
        [SerializeField]
        private float _gnusSpeedMultiplier = 0.5f;

        [SerializeField]
        private GnusView _gnusViewPrefab;

        [Inject]
        private IGameModel _gameModel;

        private readonly List<IDisposable> _subscriptions = new();

        private ObjectPool<GnusView> _gnusPool;

        private readonly List<MapPathCode> _pathsWithActiveGnus = new();

        private readonly Dictionary<MapPathCode, List<GnusView>> _activeGnus = new();

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _gnusPool = new ObjectPool<GnusView>(
                createFunc: () => Instantiate(_gnusViewPrefab),
                actionOnGet: gnus => gnus.gameObject.SetActive(true),
                actionOnRelease: gnus => gnus.gameObject.SetActive(false),
                actionOnDestroy: Destroy,
                collectionCheck: false,
                defaultCapacity: 16
            );

            _gameModel.MapModel.OpenedPaths.SubscribeChanged(OnUpdateOpenedPaths).AddToCollection(_subscriptions);

            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _subscriptions.Dispose();

            foreach (var (_, list) in _activeGnus)
            {
                foreach (var gnus in list)
                {
                    _gnusPool.Release(gnus);
                }
            }

            _activeGnus.Clear();
            _pathsWithActiveGnus.Clear();

            return UniTask.CompletedTask;
        }

        private void OnUpdateOpenedPaths()
        {
            var mapModel = _gameModel.MapModel;

            //Debug.Log($"Update Gnus {mapModel.OpenedPaths.Count}");
            
            foreach (var (pathCode, pathGnusView) in mapModel.MapView.PathGnusViews)
            {
                Debug.Log($"Update Gnus {pathCode}");
                var pathOpened = mapModel.OpenedPaths.Contains(pathCode);
                var isGnusActive = _pathsWithActiveGnus.Contains(pathCode);
                if (pathOpened)
                {
 
                    if (isGnusActive)
                    {
                        continue;
                    }

                    var list = new List<GnusView>();

                    foreach (var gnusPoint in pathGnusView.GnusPoints)
                    {
                        Debug.Log($"Create gnus");
                        var gnusView = _gnusPool.Get();
                        gnusView.transform.SetParent(gnusPoint);
                        gnusView.transform.localPosition = Vector3.zero;
                        gnusView.transform.localRotation = Quaternion.identity;
                        list.Add(gnusView);
                    }
          
                    _activeGnus[pathCode] = list;
                    _pathsWithActiveGnus.Add(pathCode);
                }
                else
                {
                    if (!isGnusActive)
                    {
                        continue;
                    }

                    if (_activeGnus.TryGetValue(pathCode, out var gnusList))
                    {
                        foreach (var gnus in gnusList)
                        {
                            _gnusPool.Release(gnus);
                        }

                        _activeGnus.Remove(pathCode);
                    }

                    _pathsWithActiveGnus.Remove(pathCode);
                }
            }
        }

        public void OnUpdate()
        {
            if (_gameModel.MapModel == null || _gameModel.MapModel.MapPlayerView == null)
            {
                return;
            }

            var defaultSpeed = _gameModel.MapModel.DefaultMovementSpeed;
            _gameModel.MapModel.MapMovementSpeed.Value = DefineIfPlayerInGnusZone()
                ? defaultSpeed * _gnusSpeedMultiplier
                : defaultSpeed;
        }

        private bool DefineIfPlayerInGnusZone()
        {
            var playerCollider = _gameModel.MapModel.MapPlayerView.BoxCollider2D;

            foreach (var list in _activeGnus.Values)
            {
                foreach (var gnusView in list)
                {
                    if (!gnusView.BoxCollider2D.enabled || !gnusView.gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    if (gnusView.BoxCollider2D.bounds.Intersects(playerCollider.bounds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

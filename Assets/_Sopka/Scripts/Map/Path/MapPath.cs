using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sopka
{
    [ExecuteAlways]
    public class MapPath : MonoBehaviour
    {
        [BoxGroup("Connection")]
        [SerializeField]
        private MapLocationCode _locationA;

        [BoxGroup("Connection")]
        [SerializeField]
        private MapLocationCode _locationB;

        [BoxGroup("Waypoints")]
        [SerializeField]
        private Transform[] _waypoints;

        [BoxGroup("LineRenderer")]
        [SerializeField]
        private LineRenderer _lineRenderer;
        
        [BoxGroup("LineRenderer")]
        [SerializeField] 
        private PathLineSettings _pathLineSettings;
        
        [BoxGroup("Gizmos")]
        [SerializeField]
        private bool _drawGizmos = true;

        [BoxGroup("Generation")]
        [SerializeField]
        private int _generatedPointsCount = 5;

        public Transform[] Waypoints => _waypoints;

        public MapLocationCode LocationA => _locationA;

        public MapLocationCode LocationB => _locationB;

        public LineRenderer LineRenderer => _lineRenderer;

        private void OnValidate()
        {
#if UNITY_EDITOR
            UpdateLineRenderer();
            #endif
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos || _waypoints == null || _waypoints.Length < 2)
            {
                return;
            }

            Handles.color = _pathLineSettings.LineColor;

            for (int i = 0; i < _waypoints.Length - 1; i++)
            {
                var a = _waypoints[i];
                var b = _waypoints[i + 1];
                if (a != null && b != null)
                {
                    Handles.DrawLine(a.position, b.position);
                    Handles.DrawSolidDisc(a.position, Vector3.forward, 0.05f);
                }
            }

            var last = _waypoints[^1];
            if (last != null)
            {
                Handles.DrawSolidDisc(last.position, Vector3.forward, 0.05f);
            }
        }

        [BoxGroup("Generation")]
        [Button("Generate Linear Waypoints")]
        private void GenerateLinearWaypoints()
        {
            Debug.Log("STARTED GENERATING");

            var mapView = ComponentFinder.FindObject<MapView>();
            Debug.Log($"MapView: {mapView}");

            if (mapView == null)
            {
                Debug.LogWarning("MapView not found in scene.");
                return;
            }

            var locationViews = mapView.MapLocationViews;
            Debug.Log($"MapView.MapLocationViews count: {locationViews.Count}");

            if (!locationViews.TryGetValue(_locationA, out var locationAView))
            {
                Debug.LogWarning("Location A not found");
                return;
            }

            if (!locationViews.TryGetValue(_locationB, out var locationBView))
            {
                Debug.LogWarning("Location B not found");
                return;
            }

            var from = locationAView.PlayerRoot;
            var to = locationBView.PlayerRoot;

            if (from == null || to == null)
            {
                Debug.LogWarning("Endpoints must be valid Transforms.");
                return;
            }

            var parent = transform;

            var newWaypoints = new Transform[_generatedPointsCount + 2];
            newWaypoints[0] = from;
            newWaypoints[^1] = to;

            for (int i = 1; i <= _generatedPointsCount; i++)
            {
                var t = new GameObject($"{name}_Waypoint_{i}").transform;
                t.position = Vector3.Lerp(from.position, to.position, i / (float)(_generatedPointsCount + 1));
                t.SetParent(parent);
                newWaypoints[i] = t;
            }

            _waypoints = newWaypoints;

            UpdateLineRenderer();
            EditorUtility.SetDirty(this);
        }

        [BoxGroup("LineRenderer")]
        [Button("Update LineRenderer")]
        private void UpdateLineRenderer()
        {
            if (_waypoints == null || _waypoints.Length < 2)
            {
                return;
            }
            if (_lineRenderer == null)
            {
                var existing = transform.GetComponentInChildren<LineRenderer>();
                if (existing != null)
                {
                    _lineRenderer = existing;
                }
                else
                {
                    var go = new GameObject("LineRenderer");
                    go.transform.SetParent(transform);
                    go.transform.localPosition = Vector3.zero;

                    _lineRenderer = go.AddComponent<LineRenderer>();
                }
            }
            _lineRenderer.positionCount = _waypoints.Length;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.widthMultiplier = _pathLineSettings.LineWidth;
            _lineRenderer.startColor = _pathLineSettings.LineColor;
            _lineRenderer.endColor = _pathLineSettings.LineColor;
            _lineRenderer.material = _pathLineSettings.LineMaterial;
            for (int i = 0; i < _waypoints.Length; i++)
            {
                if (_waypoints[i] != null)
                {
                    _lineRenderer.SetPosition(i, _waypoints[i].position);
                }
            }

        }
        
        [BoxGroup("Generation")]
        [Button("Clear Waypoints")]
        private void ClearWaypoints()
        {
            if (_waypoints == null || _waypoints.Length == 0)
            {
                return;
            }

            for (int i = 0; i < _waypoints.Length; i++)
            {
                var point = _waypoints[i];
                if (point == null)
                {
                    continue;
                }

                if (point.gameObject.name.Contains("Waypoint"))
                {
#if UNITY_EDITOR
                    if (!EditorApplication.isPlaying)
                    {
                        Object.DestroyImmediate(point.gameObject);
                    }
                    else
#endif
                    {
                        Object.Destroy(point.gameObject);
                    }
                }
            }

            _waypoints = new Transform[0];

            if (_lineRenderer != null)
            {
                _lineRenderer.positionCount = 0;
            }

            EditorUtility.SetDirty(this);
        }


#endif
    }
}

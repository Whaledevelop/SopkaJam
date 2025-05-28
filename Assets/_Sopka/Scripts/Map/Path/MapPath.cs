using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sopka
{
    [ExecuteAlways]
    public class MapPath : MonoBehaviour
    {
        [SerializeField] private MapPathCode _mapPathCode;
        
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
        private Color _lineColor = Color.yellow;

        [BoxGroup("LineRenderer")]
        [SerializeField]
        private float _lineWidth = 0.1f;

        [BoxGroup("Gizmos")]
        [SerializeField]
        private bool _drawGizmos = true;

        [BoxGroup("Generation")]
        [SerializeField]
        private int _generatedPointsCount = 5;

        public Transform[] Waypoints => _waypoints;

        public MapLocationCode LocationA => _locationA;

        public MapLocationCode LocationB => _locationB;

        public MapPathCode PathCode => _mapPathCode;

        public LineRenderer LineRenderer => _lineRenderer;

        private void OnValidate()
        {
            UpdateLineRenderer();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_drawGizmos || _waypoints == null || _waypoints.Length < 2)
            {
                return;
            }

            Handles.color = _lineColor;

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
            var mapView = FindFirstObjectByType<MapView>();
            if (mapView == null)
            {
                Debug.LogWarning("MapView not found in scene.");
                return;
            }

            var locationViews = mapView.MapLocationViews;
            if (!locationViews.TryGetValue(_locationA, out var locationAView) ||
                !locationViews.TryGetValue(_locationB, out var locationBView))
            {
                Debug.LogWarning("Map locations not found in MapView.");
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
                var existing = GetComponentInChildren<LineRenderer>();
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

            if (_roadMaterial != null)
            {
                //SetupRoadPath();
            }
            else
            {
                _lineRenderer.positionCount = _waypoints.Length;
                _lineRenderer.useWorldSpace = true;
                _lineRenderer.widthMultiplier = _lineWidth;
                _lineRenderer.startColor = _lineColor;
                _lineRenderer.endColor = _lineColor;

                for (int i = 0; i < _waypoints.Length; i++)
                {
                    if (_waypoints[i] != null)
                    {
                        _lineRenderer.SetPosition(i, _waypoints[i].position);
                    }
                }
            }
        }
        
        [SerializeField]
        private Material _roadMaterial;
        
        [Button]
        private void SetupRoadPath()
        {
            if (_lineRenderer == null || _waypoints == null || _waypoints.Length < 2)
            {
                return;
            }

            var positions = _waypoints
                .Where(w => w != null)
                .Select(w => w.position)
                .ToArray();

            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);

            _lineRenderer.material = _roadMaterial;
            _lineRenderer.textureMode = LineTextureMode.Tile;
            _lineRenderer.alignment = LineAlignment.TransformZ;

            _lineRenderer.startWidth = _lineWidth;
            _lineRenderer.endWidth = _lineWidth;

            var length = CalculateLineLength(positions);
            _lineRenderer.material.mainTextureScale = new Vector2(length * 0.5f, 1f);
        }

        private float CalculateLineLength(Vector3[] positions)
        {
            float totalLength = 0f;
            for (int i = 1; i < positions.Length; i++)
            {
                totalLength += Vector3.Distance(positions[i - 1], positions[i]);
            }

            return totalLength;
        }

#endif
    }
}

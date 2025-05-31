using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/FogOfWarSystem", fileName = "FogOfWarSystem")]
    public class FogOfWarSystem : GameSystem, IUpdate
    {
        [BoxGroup("Fog Settings")]
        [SerializeField]
        private RenderTexture _fogMask;

        [BoxGroup("Fog Settings")]
        [SerializeField]
        private Texture2D _revealTexture;

        [BoxGroup("Fog Settings")]
        [SerializeField]
        private Material _blitMaterial;

        [BoxGroup("Reveal Settings")]
        [SerializeField]
        private float _revealRadius = 5f;

        [Inject]
        private IGameModel _gameModel;

        private Vector2 _fogWorldSize;

        private Vector2 _fogWorldOrigin;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            var mapView = _gameModel.MapModel.MapView;
            var bounds = mapView.MapSpriteRenderer.bounds;

            _fogWorldOrigin = bounds.min;
            _fogWorldSize = bounds.size;

            Graphics.SetRenderTarget(_fogMask);
            GL.Clear(true, true, Color.black);
            Graphics.SetRenderTarget(null);

            var pos = _gameModel.MapModel.MapPlayerView.transform.position;
            RevealAtPosition(pos, _revealRadius);

            return UniTask.CompletedTask;
        }

        public void OnUpdate()
        {
            if (_gameModel.MapModel == null)
            {
                return;
            }

            if (_gameModel.MapModel.MapPlayerState.Value == MapPlayerState.Static)
            {
                return;
            }

            var pos = _gameModel.MapModel.MapPlayerView.transform.position;
            RevealAtPosition(pos, _revealRadius);
        }

        private void RevealAtPosition(Vector2 worldPos, float radius)
        {
            var center = WorldToUV(worldPos);
            var size = Mathf.RoundToInt(radius * (_fogMask.width / _fogWorldSize.x));

            var rect = new Rect(
                center.x - size,
                center.y - size,
                size * 2,
                size * 2
            );

            Graphics.SetRenderTarget(_fogMask);
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, _fogMask.width, 0, _fogMask.height);

            Graphics.DrawTexture(rect, _revealTexture, _blitMaterial);

            GL.PopMatrix();
            Graphics.SetRenderTarget(null);
        }

        private Vector2Int WorldToUV(Vector2 worldPos)
        {
            var local = worldPos - _fogWorldOrigin;

            var normalized = new Vector2(
                Mathf.Clamp01(local.x / _fogWorldSize.x),
                Mathf.Clamp01(local.y / _fogWorldSize.y)
            );

            var uv = new Vector2Int(
                Mathf.RoundToInt(normalized.x * _fogMask.width),
                Mathf.RoundToInt(normalized.y * _fogMask.height)
            );

            return uv;
        }
    }
}

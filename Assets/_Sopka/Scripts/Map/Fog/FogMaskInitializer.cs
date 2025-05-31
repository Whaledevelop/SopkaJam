using UnityEngine;

namespace Sopka
{
    public class FogMaskInitializer : MonoBehaviour
    {
        [SerializeField]
        private RenderTexture _fogMask;

        [SerializeField]
        private Texture2D _revealTexture;

        [SerializeField]
        private Material _blitMaterial;

        [SerializeField]
        private Vector2Int _center = new(256, 128); // по центру RenderTexture

        [SerializeField]
        private int _radius = 64;

        private void Awake()
        {
            if (_fogMask == null)
            {
                return;
            }

            Graphics.SetRenderTarget(_fogMask);
            GL.Clear(true, true, Color.black);
            Graphics.SetRenderTarget(null);

            DrawCirclePixels(_fogMask, _center, _radius);
        }
        
        private void DrawCirclePixels(RenderTexture target, Vector2Int center, int radius)
        {
            var tex = new Texture2D(target.width, target.height, TextureFormat.R8, false);
    
            // Скопировать текущую маску
            RenderTexture.active = target;
            tex.ReadPixels(new Rect(0, 0, target.width, target.height), 0, 0);
            tex.Apply();

            var rSqr = radius * radius;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    var dx = center.x + x;
                    var dy = center.y + y;

                    if (dx < 0 || dx >= target.width || dy < 0 || dy >= target.height)
                    {
                        continue;
                    }

                    if (x * x + y * y <= rSqr)
                    {
                        tex.SetPixel(dx, dy, Color.white);
                    }
                }
            }

            tex.Apply();

            Graphics.Blit(tex, target);

            RenderTexture.active = null;
            Object.Destroy(tex);
        }


    }
}
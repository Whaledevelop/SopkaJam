using UnityEngine;

namespace Sopka
{
    public class ObjectOnMapRendererView : ObjectOnMapView
    {
        [SerializeField] private Renderer _renderer;
        
        public override void SetMode(bool mode)
        {
            _renderer.enabled = mode;
        }
    }
}
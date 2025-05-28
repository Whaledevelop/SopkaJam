using System;
using UnityEngine;

namespace Sopka
{
    public class ObjectOnMapView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        public Action OnMouseDownEvent;

        public Renderer Renderer => _renderer;

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
        }
    }
}
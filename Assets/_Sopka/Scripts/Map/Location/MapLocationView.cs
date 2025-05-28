using System;
using UnityEngine;

namespace Sopka
{
    public class MapLocationView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        [SerializeField] private Transform _playerRoot;
        
        public Action OnMouseDownEvent;
        
        public Transform PlayerRoot => _playerRoot;

        public Renderer Renderer => _renderer;

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
        }
    }
}
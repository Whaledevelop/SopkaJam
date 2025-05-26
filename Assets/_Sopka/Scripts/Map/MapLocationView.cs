using System;
using UnityEngine;

namespace Sopka
{
    public class MapLocationView : MonoBehaviour
    {
        [SerializeField] private Transform _playerRoot;
        
        public event Action OnMouseDownEvent;
        
        public Transform PlayerRoot => _playerRoot;

        private void OnMouseDown()
        {
            OnMouseDownEvent?.Invoke();
        }
    }
}
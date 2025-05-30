using System;
using UnityEngine;

namespace Sopka
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class ObjectOnMapView : MonoBehaviour
    {
        // public Action OnMouseDownEvent;
        //
        // private void OnMouseDown()
        // {
        //     OnMouseDownEvent?.Invoke();
        // }

        public abstract void SetMode(bool mode);
    }
}
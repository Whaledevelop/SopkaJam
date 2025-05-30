using UnityEngine;

namespace Sopka
{
    public class MapPlayerView : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D _boxCollider2D;


        public BoxCollider2D BoxCollider2D => _boxCollider2D;
    }
}
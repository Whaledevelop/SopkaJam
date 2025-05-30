using UnityEngine;

namespace Sopka
{
    public class GnusView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private BoxCollider2D _boxCollider2D;

        public ParticleSystem ParticleSystem => _particleSystem;

        public BoxCollider2D BoxCollider2D => _boxCollider2D;
    }
}
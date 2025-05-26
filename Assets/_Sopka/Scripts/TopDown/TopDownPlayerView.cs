using UnityEngine;

namespace Sopka
{
    public class TopDownPlayerView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;

        public Animator Animator => _animator;
        public Rigidbody2D Rigidbody => _rigidbody;
        public Transform Transform => transform;
    }
}
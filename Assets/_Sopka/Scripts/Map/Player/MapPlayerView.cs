using UnityEngine;

namespace Sopka
{
    public class MapPlayerView : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public Animator Animator => _animator;
    }
}
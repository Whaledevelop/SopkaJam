using System;
using UnityEngine;

namespace Sopka
{
    [Serializable]
    public struct ResourceStartValues
    {
        [SerializeField]
        private int _teamMembers;

        [SerializeField]
        private int _supplies;

        [SerializeField]
        private float _hungerProgress;

        [SerializeField]
        private bool _hungerActive;

        [SerializeField]
        private float _hungerDefaultSpeed;

        public int TeamMembers => _teamMembers;
        public int Supplies => _supplies;
        public float HungerProgress => _hungerProgress;
        public bool HungerActive => _hungerActive;
        public float HungerDefaultSpeed => _hungerDefaultSpeed;
    }
}
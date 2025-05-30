using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sopka
{
    public class PathGnusView : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _gnusPoints;

        public Transform[] GnusPoints => _gnusPoints;
    }
}
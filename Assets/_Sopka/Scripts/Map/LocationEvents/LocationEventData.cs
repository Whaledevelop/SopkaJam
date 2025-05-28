using System;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class LocationEventData
    {
        [SerializeField] 
        private MapLocationCode _mapLocationCode;

        [SerializeReference] 
        private IAction _action;

        [SerializeField] 
        private float _timeoutBeforeAction = 0f;

        [SerializeField] private ConditionCode[] _requiredConditions;

        public MapLocationCode LocationCode => _mapLocationCode;

        public IAction Action => _action;

        public float TimeoutBeforeAction => _timeoutBeforeAction;

        public ConditionCode[] RequiredConditions => _requiredConditions;
    }
}
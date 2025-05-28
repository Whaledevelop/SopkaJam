using System;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ObjectOnMapEventData
    {
        [SerializeField] 
        private ObjectOnMapCode _code;
        
        [SerializeReference] 
        private IAction _action;
        
        public ObjectOnMapCode Code => _code;
        
        public IAction Action => _action;
    }
}
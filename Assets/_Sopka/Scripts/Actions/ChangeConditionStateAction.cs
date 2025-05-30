using System;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeConditionStateAction : IAction
    {
        [FormerlySerializedAs("_mapConditionCode")] [SerializeField]
        private ConditionCode _conditionCode;

        [SerializeField] private bool _state = true;
        [Inject]
        private IGameModel _gameModel;
        
        public void Execute()
        {
            if (_state)
            {
                _gameModel.MapModel.FulfilledConditions.Add(_conditionCode);
            }
            else
            {
                _gameModel.MapModel.FulfilledConditions.Remove(_conditionCode);
            }
        }
    }
}
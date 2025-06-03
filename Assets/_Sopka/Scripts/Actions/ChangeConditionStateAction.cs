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
        
        private IGameModel _gameModel;
        
        [Inject]
        private void Construct(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }
        
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
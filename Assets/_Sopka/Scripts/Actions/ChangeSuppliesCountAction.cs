using System;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeSuppliesCountAction : IAction
    {
        [SerializeField] private int _changeCount;
        
        [Inject] private IGameModel _gameModel;
        
        public void Execute()
        {
            _gameModel.ResourcesModel.SuppliesCount.Value = Mathf.Clamp(_gameModel.ResourcesModel.SuppliesCount.Value + _changeCount, 0, int.MaxValue);
        }
    }
}
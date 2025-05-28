using System;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class OpenLocationAndPathAction : IAction
    {
        [SerializeField]
        private MapLocationCode _mapLocationCode;
        
        [SerializeField]
        private MapPathCode _mapPathCode;
        
        [Inject]
        private IGameModel _gameModel;
        
        public void Execute()
        {
            _gameModel.MapModel.OpenedPaths.Add(_mapPathCode);
            _gameModel.MapModel.OpenedLocations.Add(_mapLocationCode);

            Debug.Log($"Add {_mapLocationCode}");
        }
    }
}
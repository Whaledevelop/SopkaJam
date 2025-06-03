using System;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeTeamCountAction : IAction
    {
        [SerializeField] private int _changeCount;
        
        private IGameModel _gameModel;
        
        [Inject]
        private void Construct(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }
        
        public void Execute()
        {
            _gameModel.ResourcesModel.TeamMembersCount.Value = Mathf.Clamp(_gameModel.ResourcesModel.TeamMembersCount.Value + _changeCount, 0, int.MaxValue);
        }
    }
}
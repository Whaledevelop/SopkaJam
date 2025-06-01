using System;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeTeamCountAction : IAction
    {
        [SerializeField] private int _changeCount;
        
        [Inject] private IGameModel _gameModel;
        
        public void Execute()
        {
            _gameModel.ResourcesModel.TeamMembersCount.Value = Mathf.Clamp(_gameModel.ResourcesModel.TeamMembersCount.Value + _changeCount, 0, int.MaxValue);
        }
    }
}
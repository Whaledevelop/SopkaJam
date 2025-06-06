﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;

namespace Sopka
{
    [Serializable]
    public class ChangeObjectOnMapStateAction : AsyncAction
    {
        [SerializeField] 
        private ObjectOnMapCode _code;

        [SerializeField] private bool _state;

        private IGameModel _gameModel;
        
        [Inject]
        private void Construct(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_state)
            {
                _gameModel.MapModel.ActiveObjects.Add(_code);
            }
            else
            {
                _gameModel.MapModel.ActiveObjects.Remove(_code);
            }
            return UniTask.CompletedTask;
        }
    }
}
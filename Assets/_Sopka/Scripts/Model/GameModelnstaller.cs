using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Whaledevelop.DiContainer;

namespace Sopka
{
    [Serializable]
    public class GameModelInstaller : MonoInstaller
    {
        [SerializeField]
        private ModelStartSettings _modelStartSettings;
        
        public override void InstallBindings()
        {
            Debug.Log("GameModelInstaller InstallBindings");
            var gameModel = new GameModel(_modelStartSettings);
            Container.BindToInterface<IGameModel>(gameModel);
        }
    }
}
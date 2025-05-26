using System;
using Whaledevelop.DiContainer;

namespace Sopka
{
    [Serializable]
    public class GameModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var gameModel = new GameModel();
            Container.BindToInterface<IGameModel>(gameModel);
        }
    }
}
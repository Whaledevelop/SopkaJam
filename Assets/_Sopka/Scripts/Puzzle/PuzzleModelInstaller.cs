using System;
using UnityEngine;
using Whaledevelop.DiContainer;

namespace Sopka
{
    [Serializable]
    public class PuzzleModelInstaller : MonoInstaller
    {
        [SerializeField]
        private Sprite[] _sprites;

        [SerializeField] private Sprite _hintSprite;
        
        public override void InstallBindings()
        {
            var puzzleModel = new PuzzleModel(_sprites, _hintSprite);
            Container.BindToInterface<IPuzzleModel>(puzzleModel);
        }
    }
}
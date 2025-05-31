using System;
using UnityEngine;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Sopka
{
    public class ReceiveItemUIViewModel : IUIViewModel
    {
        public ReceiveItemUIViewModel(string text, Sprite sprite, Action onClickContinue)
        {
            OnClickContinue = onClickContinue;
            ItemText = new ReactiveValue<string>(text);
            ItemSprite = new ReactiveValue<Sprite>(sprite);
        }
        
        public ReactiveValue<string> ItemText { get; }

        public ReactiveValue<Sprite> ItemSprite { get; }

        public Action OnClickContinue;
    }
}
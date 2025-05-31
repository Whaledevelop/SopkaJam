using System;
using System.Collections.Generic;
using UnityEngine;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Whaledevelop.Dialogs.UI
{
    public class DialogViewModel : IUIViewModel
    {
        public DialogViewModel(Sprite[] speakerSprites, Sprite startBackground, Action onClickNext, Action<int> onOptionSelected, float textAppendInterval, Action onClickSkipDialog)
        {
            SpeakerSprites = speakerSprites;
            OnClickNext = onClickNext;
            OnOptionSelected = onOptionSelected;
            TextAppendInterval = textAppendInterval;
            OnClickSkipDialog = onClickSkipDialog;
            DialogLine = new ReactiveValue<(string, string)>();
            Options = new ReactiveCollection<string>();
            BackgroundSprite = new ReactiveValue<Sprite>(startBackground);
        }

        public ReactiveValue<(string name, string phrase)> DialogLine { get; }

        public ReactiveCollection<string> Options { get; }

        public ReactiveValue<Sprite> BackgroundSprite { get; }
        
        public Sprite[] SpeakerSprites { get; }
        
        public Action<int> OnOptionSelected { get; }
        
        public Action OnClickNext { get; }
        
        public Action OnClickSkipDialog { get; }
        public float TextAppendInterval { get; }
    }
}
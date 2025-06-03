using System;
using System.Collections.Generic;
using Sopka;
using TMPro;
using UnityEngine;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Whaledevelop.Dialogs.UI
{
    public class DialogViewModel : IUIViewModel
    {
        public DialogViewModel(Sprite startBackground, Action onClickNext, Action<int> onOptionSelected, 
            float textAppendInterval, Action onClickSkipDialog, Action<ItemCode> onClickItem)
        {
            OnClickNext = onClickNext;
            OnOptionSelected = onOptionSelected;
            TextAppendInterval = textAppendInterval;
            OnClickSkipDialog = onClickSkipDialog;
            FontStyle = new ReactiveValue<FontStyles>(FontStyles.Normal);
            Options = new ReactiveCollection<string>();
            BackgroundSprite = new ReactiveValue<Sprite>(startBackground);

            ItemsStatuses = new ReactiveDictionary<ItemCode, bool>();

            OnClickItem = onClickItem;
            SpeakerName = new ReactiveValue<string>();
            MainText = new ReactiveValue<string>();
            SpeakerSprite = new ReactiveValue<Sprite>();
        }

        public ReactiveValue<string> SpeakerName { get; }
        public ReactiveValue<string> MainText { get; }

        public ReactiveValue<Sprite> SpeakerSprite { get; }
        public ReactiveValue<FontStyles> FontStyle { get; }
        public ReactiveCollection<string> Options { get; }

        public ReactiveValue<Sprite> BackgroundSprite { get; }

        public ReactiveDictionary<ItemCode, bool> ItemsStatuses { get; }
        
        public Action<int> OnOptionSelected { get; }
        
        public Action OnClickNext { get; }
        
        public Action OnClickSkipDialog { get; }
        public float TextAppendInterval { get; }
        
        public Action<ItemCode> OnClickItem { get; }
        
        public ReactiveCommand<bool> ChangeDownPanelCommand { get; } = new ReactiveCommand<bool>();
    }
}
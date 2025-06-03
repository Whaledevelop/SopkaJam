using System;
using System.Collections.Generic;
using Sopka;
using UnityEngine;
using Whaledevelop.Dialogs.UI;
using Whaledevelop.UI;
using Whaledevelop.Utility;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    public class ReceiveItemData
    {
        public string Text;

        public Sprite Icon;
    }
    
    [Serializable]
    public class DialogUISettings
    {
        [SerializeField]
        private DialogView _dialogView;

        [SerializeField]
        private float _textAppendInterval = 0.03f;

        [SerializeField]
        private float _startDialogDelay = 0.5f;

        [SerializeField]
        private float _nextLineDelay = 1.0f;

        [SerializeField] private SpeakerSettings _narratorSettings;

        [SerializeField] private ReceiveItemUIView _receiveItemUI;

        [SerializeField] private SerializableDictionary<ItemCode, ReceiveItemData> _itemsData;

        public DialogView DialogView => _dialogView;

        public float TextAppendInterval => _textAppendInterval;

        public float StartDialogDelay => _startDialogDelay;

        public float NextLineDelay => _nextLineDelay;

        public SpeakerSettings NarratorSettings => _narratorSettings;

        public IReadOnlyDictionary<ItemCode, ReceiveItemData> ItemsData => _itemsData;

        public ReceiveItemUIView ReceiveItemUI => _receiveItemUI;
    }
}
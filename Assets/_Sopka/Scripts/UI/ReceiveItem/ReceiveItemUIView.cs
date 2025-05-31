using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whaledevelop;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Sopka
{
    public class ReceiveItemUIView : UIView<ReceiveItemUIViewModel>
    {
        [SerializeField]
        private Image _itemImage;

        [SerializeField]
        private TextMeshProUGUI _itemText;

        [SerializeField]
        private Button _continueButton;

        private readonly List<IDisposable> _subscriptions = new List<IDisposable>();
            
        public override void Initialize()
        {
            _continueButton.onClick.AddListener(() =>
            {
                DerivedModel.OnClickContinue?.Invoke();
            });

            DerivedModel.ItemText.Subscribe(UpdateText).AddToCollection(_subscriptions);
            DerivedModel.ItemSprite.Subscribe(UpdateImage).AddToCollection(_subscriptions);
        }

        public override void Release()
        {
            _continueButton.onClick.RemoveAllListeners();
            _subscriptions.Dispose();
        }

        private void UpdateText(string value)
        {
            _itemText.text = value;
        }

        private void UpdateImage(Sprite sprite)
        {
            _itemImage.sprite = sprite;
        }
    }
}
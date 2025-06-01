using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sopka;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Whaledevelop.Reactive;
using Whaledevelop.UI;
using Whaledevelop.Utility;

namespace Whaledevelop.Dialogs.UI
{
    public class DialogView : UIView<DialogViewModel>
    {
        [SerializeField]
        private Image _speakerImage;

        [SerializeField] 
        private GameObject _textRootGameObject;
        
        [SerializeField]
        private TextMeshProUGUI _textLabel;

        [SerializeField] private TextMeshProUGUI _narratorLabel;
        
        [SerializeField]
        private TextMeshProUGUI _speakerNameLabel;

        [SerializeField]
        private Button[] _optionsButtons;
        
        [SerializeField]
        private TextMeshProUGUI[] _optionsTexts;
        
        [SerializeField]
        private Button _nextButton;
        
        [SerializeField]
        private Image _backgroundImage;

        [SerializeField] 
        private Button _skipDialogButton;
        
        [SerializeField]
        private SerializableDictionary<ItemCode, ItemView> _itemsViews;

        [SerializeField] private GameObject _speakerSpritesRoot;

        [SerializeField] private GameObject _narratorTextRoot;

        [SerializeField] private Button _narratorNext;

        [SerializeField] private SpeakerSettings _mainCharacterSettings;

        private readonly List<IDisposable> _subscriptions = new();

        private CancellationTokenSource _textCancellation;

        public override void Initialize()
        {
            ClearOptions();
            
            _skipDialogButton.onClick.AddListener(OnSkip); // для тестов
            
            _nextButton.onClick.AddListener(OnClickNextButton);
            _narratorNext.onClick.AddListener(OnClickNextButton);
            
            SetNextButtonEnabled(true);
            
            DerivedModel.MainText.Subscribe(OnChangeText).AddToCollection(_subscriptions);
            DerivedModel.SpeakerName.Subscribe(OnChangeSpeakerName).AddToCollection(_subscriptions);
            DerivedModel.SpeakerSprite.Subscribe(OnChangeSpeakerSprite).AddToCollection(_subscriptions);
            
            DerivedModel.FontStyle.Subscribe(OnChangeFontStyle).AddToCollection(_subscriptions);
            DerivedModel.Options.SubscribeChanged(OnChangeOptions).AddToCollection(_subscriptions);
            DerivedModel.BackgroundSprite.Subscribe(OnSetBackgroundSprite).AddToCollection(_subscriptions);
        
            DerivedModel.ItemsStatuses.SubscribeChanged(OnUpdateStatuses).AddToCollection(_subscriptions);

            foreach (var (itemCode, itemView) in _itemsViews)
            {
                itemView.Button.onClick.AddListener(() => DerivedModel.OnClickItem(itemCode));
            }
        }

        private CancellationTokenSource _statusesCts;

        private void OnChangeText(string text)
        {
            //SetNextButtonEnabled(false);
            _textCancellation?.Cancel();
            _textCancellation = new CancellationTokenSource();

            if (string.IsNullOrEmpty(text))
            {
                _textLabel.text = null;

                return;
            }

            var isNarrator = string.IsNullOrEmpty(_speakerNameLabel.text);
            _textRootGameObject.SetActive(!isNarrator);
            _narratorTextRoot.SetActive(isNarrator);
            ShowTextAsync(_narratorLabel, text, _textCancellation.Token).Forget();
            ShowTextAsync(_textLabel, text, _textCancellation.Token).Forget();
        }

        private void OnChangeSpeakerName(string speakerName)
        {
            _speakerNameLabel.text = speakerName;
        }

        private void OnChangeSpeakerSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                _speakerSpritesRoot.SetActive(false);
            }
            else
            {
                _speakerImage.sprite = sprite;
                _speakerSpritesRoot.SetActive(true);
            }
        }

        private void OnUpdateStatuses()
        {
            _statusesCts?.Cancel();
            _statusesCts = new CancellationTokenSource();

            foreach (var (itemCode, itemView) in _itemsViews)
            {
                var root = itemView.Root;
                var canvasGroup = itemView.CanvasGroup;
                canvasGroup.alpha = 0f;

                DOTween.Kill(root);

                if (DerivedModel.ItemsStatuses.TryGetValue(itemCode, out var itemStatus) && itemStatus)
                {
                    itemView.gameObject.SetActive(true);

                    var token = _statusesCts.Token;

                    canvasGroup.DOFade(1f, 0.3f).OnComplete(() =>
                    {
                        DOTweenUtility.LoopScale(root, token).Forget();
                    });
                }
                else
                {
                    canvasGroup.DOFade(0f, 0.2f).OnComplete(() =>
                    {
                        itemView.gameObject.SetActive(false);
                    });
                }
            }
        }

        
        private void OnChangeFontStyle(FontStyles fontStyle)
        {
            _narratorLabel.fontStyle = fontStyle;
            _textLabel.fontStyle = fontStyle;
        }

        private void OnSkip()
        {
            Release();
            DerivedModel.OnClickSkipDialog?.Invoke();
        }

        private bool _released;

        public override void Release()
        {
            if (_released)
            {
                return;
            }
            _subscriptions.Dispose();
            _textCancellation?.Cancel();
            _textCancellation?.Dispose();
            _released = true;
        }

        private void OnSetBackgroundSprite(Sprite sprite)
        {
            _backgroundImage.sprite = sprite;
        }
        
        private void OnClickNextButton()
        {
            _textCancellation?.Cancel();
            //_textCancellation?.Dispose();
            _textLabel.text = DerivedModel.MainText.Value;
            _narratorLabel.text = DerivedModel.MainText.Value;
            
            UniTaskUtility.ExecuteAfterSeconds(0.2f, () => DerivedModel.OnClickNext?.Invoke(), CancellationToken.None);
        }

        private void OnChangeOptions()
        {
            ClearOptions();
            if (_narratorTextRoot.activeSelf)
            {
                _textLabel.text = _narratorLabel.text;
                _textRootGameObject.SetActive(true);
                _narratorTextRoot.SetActive(false);
            }
            // OnChangeSpeakerSprite(_mainCharacterSettings.Icon);
            // _speakerNameLabel.text = _mainCharacterSettings.GetNameText();
            // Debug.Log($"On change options {_speakerImage.sprite}");
            foreach (var (optionText, index) in DerivedModel.Options.Select((x, i) => (x, i)))
            {
                _optionsTexts[index].text = optionText;
                var capturedIndex = index;
                
                
                _textRootGameObject.SetActive(true);
                _narratorTextRoot.SetActive(false);
                _optionsButtons[index].onClick.AddListener(() => DerivedModel.OnOptionSelected?.Invoke(capturedIndex));
                _optionsButtons[index].gameObject.SetActive(true);
            }

            //SetNextButtonEnabled(false);
        }

        private void ClearOptions()
        {
            foreach (var button in _optionsButtons)
            {
                button.onClick.RemoveAllListeners();
                button.gameObject.SetActive(false);
            }
            
        }

        private void OnTextShown()
        {
            if (_released)
            {
                return;
            }

            //SetNextButtonEnabled(true);
        }

        private void SetNextButtonEnabled(bool mode)
        {
            if (string.IsNullOrEmpty(_speakerNameLabel.text))
            {
                _narratorNext.gameObject.SetActive(mode);
            }
            else
            {
                _nextButton.gameObject.SetActive(mode);
            }
        }

        private async UniTask ShowTextAsync(TextMeshProUGUI text, string phrase, CancellationToken cancellationToken)
        {
            text.text = string.Empty;

            var sequence = TextAnimationUtility.BuildTypewriterSequence(text, phrase, DerivedModel.TextAppendInterval);

            cancellationToken.Register(() => sequence.Kill());

            await sequence.AsyncWaitForCompletion();

            OnTextShown();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Whaledevelop.Dialogs.UI
{
    public class DialogView : UIView<DialogViewModel>
    {
        [SerializeField]
        private Image[] _speakersImages;

        [SerializeField]
        private Image _rightImage;

        [SerializeField] 
        private GameObject _textRootGameObject;
        
        [SerializeField]
        private TextMeshProUGUI _textLabel;

        [SerializeField]
        private RectTransform _optionsContainer;

        [SerializeField]
        private Button _optionButtonPrefab;
        
        [FormerlySerializedAs("_nextButtonPrefab")] [SerializeField]
        private Button _nextButton;
        
        [SerializeField]
        private Image _backgroundImage;

        [SerializeField] private Button _skipDialogButton;

        private readonly List<Button> _optionButtons = new();
        private readonly List<IDisposable> _subscriptions = new();

        private CancellationTokenSource _textCancellation;

        public override void Initialize()
        {
            ClearOptions();

            for (var i = 0; i < _speakersImages.Length; i++)
            {
                if (DerivedModel.SpeakerSprites.Length > i && DerivedModel.SpeakerSprites[i] != null)
                {
                    _speakersImages[i].sprite = DerivedModel.SpeakerSprites[i];
                    _speakersImages[i].enabled = true;
                }
                else
                {
                    _speakersImages[i].enabled = false;
                }
            }

            _skipDialogButton.onClick.AddListener(OnSkip); // для тестов
            
            _nextButton.onClick.AddListener(OnClickNextButton);
            _nextButton.gameObject.SetActive(false);
            DerivedModel.DialogLine.Subscribe(OnChangeLine).AddToCollection(_subscriptions);
            DerivedModel.Options.SubscribeChanged(OnChangeOptions).AddToCollection(_subscriptions);
            DerivedModel.BackgroundSprite.Subscribe(OnSetBackgroundSprite).AddToCollection(_subscriptions);
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

        private void OnChangeLine((string speakerName, string phrase) line)
        {
            _nextButton.gameObject.SetActive(false);
            _textCancellation?.Cancel();
            _textCancellation = new CancellationTokenSource();

            if (string.IsNullOrEmpty(line.phrase))
            {
                _textLabel.text = null;

                return;
            }

            _textRootGameObject.SetActive(true);
            ShowTextAsync(line.speakerName, line.phrase, _textCancellation.Token).Forget();
        }
        
        private void OnClickNextButton()
        {
            _textCancellation?.Cancel();
            //_textCancellation?.Dispose();
            DerivedModel.OnClickNext?.Invoke();
        }

        private void OnChangeOptions()
        {
            ClearOptions();

            foreach (var (optionText, index) in DerivedModel.Options.Select((x, i) => (x, i)))
            {
                var button = Instantiate(_optionButtonPrefab, _optionsContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = optionText;

                var capturedIndex = index;
                button.onClick.AddListener(() => DerivedModel.OnOptionSelected?.Invoke(capturedIndex));

                _optionButtons.Add(button);
            }
            
            _nextButton.gameObject.SetActive(false);
        }

        private void ClearOptions()
        {
            foreach (var button in _optionButtons)
            {
                Destroy(button.gameObject);
            }

            _optionButtons.Clear();
        }

        private void OnTextShown()
        {
            if (_released)
            {
                return;
            }
            _nextButton.gameObject.SetActive(true);
        }

        private async UniTask ShowTextAsync(string speakerName, string phrase, CancellationToken cancellationToken)
        {
            var speakerText = string.IsNullOrEmpty(speakerName) ? string.Empty : $"{speakerName} : ";

            _textLabel.text = speakerText;

            var sequence = TextAnimationUtility.BuildTypewriterSequence(_textLabel, phrase, DerivedModel.TextAppendInterval);

            cancellationToken.Register(() => sequence.Kill());

            await sequence.AsyncWaitForCompletion();

            OnTextShown();
        }
    }
}

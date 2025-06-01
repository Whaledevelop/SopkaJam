using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whaledevelop;
using Whaledevelop.UI;

namespace Sopka.UI.MapPopup
{
    public class MapPopupView : UIView<MapPopupViewModel>
    {
        [SerializeField] private TextMeshProUGUI _text;

        [SerializeField] private Button[] _buttons;

        [SerializeField] private TextMeshProUGUI[] _buttonsTexts;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        public override void Initialize()
        {
            if (DerivedModel.AnimatedText)
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource = new CancellationTokenSource();
                SetTextAsync(_cancellationTokenSource.Token).Forget();

            }
            else
            {
                _text.text = DerivedModel.Text;
            }

            foreach (var button in _buttons)
            {
                button.onClick.AddListener(OnClick);
            }

            foreach (var buttonText in _buttonsTexts)
            {
                buttonText.text = DerivedModel.ButtonText;
            }
        }

        public override void Release()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            foreach (var button in _buttons)
            {
                button.onClick.RemoveListener(OnClick);
            }
        }

        private async UniTask SetTextAsync(CancellationToken cancellationToken)
        {
            var sequence = TextAnimationUtility.BuildTypewriterSequence(_text, DerivedModel.Text, DerivedModel.AnimationInterval);

            cancellationToken.Register(() => sequence.Kill());

            await sequence.AsyncWaitForCompletion();
        }

        private void OnClick()
        {
            DerivedModel.OnClick?.Invoke();
        }
    }
}
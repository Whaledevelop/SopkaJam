using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whaledevelop.UI;
using DG.Tweening;

namespace Sopka
{
    public class StartMenuView : UIView<StartMenuViewModel>
    {
        [SerializeField]
        private Button _startButton;

        [SerializeField]
        private TextMeshProUGUI _startText;

        [SerializeField]
        private Button _quitButton;

        [SerializeField]
        private TextMeshProUGUI _quitText;

        [SerializeField]
        private float _animationScale = 0.9f;

        [SerializeField]
        private float _animationDuration = 0.1f;

        public override void Initialize()
        {
            _startButton.onClick.AddListener(OnStartClick);
            _quitButton.onClick.AddListener(OnQuitClick);
        }

        private void OnStartClick()
        {
            AnimateText(_startText, DerivedModel.OnClickStart);
        }

        private void OnQuitClick()
        {
            AnimateText(_quitText, DerivedModel.OnClickQuit);
        }

        private void AnimateText(TextMeshProUGUI text, System.Action callback)
        {
            var sequence = DOTween.Sequence();

            sequence.Append(text.transform.DOScale(_animationScale, _animationDuration))
                .Append(text.transform.DOScale(1f, _animationDuration))
                .OnComplete(() => callback?.Invoke());
        }
    }
}
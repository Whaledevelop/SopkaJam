using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whaledevelop;
using Whaledevelop.Reactive;
using Whaledevelop.UI;

namespace Sopka
{
    public class MapHUDView : UIView<MapHUDViewModel>
    {
        [SerializeField]
        private TextMeshProUGUI _teamMembersCountText;

        [SerializeField]
        private RectTransform _teamMembersTextRoot;

        [SerializeField]
        private RectTransform _teamMembersChangePoint;

        [SerializeField]
        private TextMeshProUGUI _suppliesCountText;

        [SerializeField]
        private RectTransform _suppliesTextRoot;

        [SerializeField]
        private RectTransform _suppliesChangePoint;

        [SerializeField]
        private TextMeshProUGUI _changeTextPrefab;

        [SerializeField]
        private Image _hungerProgressImage;

        [SerializeField]
        private float _changeTextDuration = 1f;

        [SerializeField]
        private Button _exitButton;
        
        private int _lastTeamMembers = -1;
        private int _lastSupplies = -1;

        private readonly List<IDisposable> _subscriptions = new();

        public override void Initialize()
        {
            DerivedModel.TeamMembersCount.Subscribe(value =>
            {
                AnimateCountChange(
                    _teamMembersCountText,
                    _teamMembersTextRoot,
                    _teamMembersChangePoint,
                    ref _lastTeamMembers,
                    value);
            }).AddToCollection(_subscriptions);

            DerivedModel.SuppliesCount.Subscribe(value =>
            {
                AnimateCountChange(
                    _suppliesCountText,
                    _suppliesTextRoot,
                    _suppliesChangePoint,
                    ref _lastSupplies,
                    value);
            }).AddToCollection(_subscriptions);

            DerivedModel.HungerProgress.Subscribe(x => _hungerProgressImage.fillAmount = x).AddToCollection(_subscriptions);
            
            _exitButton.onClick.AddListener(OnClickExit);
        }

        private void OnClickExit()
        {
            DerivedModel.OnClickExit?.Invoke();
        }

        private void AnimateCountChange(
            TextMeshProUGUI mainText,
            RectTransform textRoot,
            RectTransform changePoint,
            ref int lastValue,
            int newValue)
        {
            if (lastValue == -1)
            {
                lastValue = newValue;
                mainText.text = newValue.ToString();
                return;
            }

            var diff = newValue - lastValue;
            lastValue = newValue;
            mainText.text = newValue.ToString();

            if (diff == 0)
            {
                return;
            }

            var instance = Instantiate(_changeTextPrefab, textRoot);
            instance.gameObject.SetActive(true);
            instance.text = diff > 0 ? $"+{diff}" : diff.ToString();
            instance.color = diff > 0 ? Color.green : Color.red;

            var startPos = diff > 0 ? changePoint.position : mainText.rectTransform.position;
            var endPos = diff > 0 ? mainText.rectTransform.position : changePoint.position;

            var rect = instance.rectTransform;
            rect.position = startPos;

            DOTween.Sequence()
                .Join(rect.DOMove(endPos, _changeTextDuration))
                .Join(instance.DOFade(0f, _changeTextDuration))
                .OnComplete(() =>
                {
                    Destroy(instance.gameObject);
                });
        }

        public override void Release()
        {
            _subscriptions.Dispose();
        }
    }
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sopka.UI.MapPopup;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.UI;

namespace Sopka
{
    [Serializable]
    public class ShowPopupAsyncAction : AsyncAction
    {
        [SerializeField] private string _text;
        
        [SerializeField] private MapPopupView _mapPopupViewPrefab;

        [SerializeField] private bool _animatedText = true;

        [SerializeField] private float _appendTextInterval = 0.05f;
        
        [Inject] private IUIService _uiService;
        
        private MapPopupViewModel _mapPopupViewModel;

        private bool _popupClosed;
        
        public override UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _mapPopupViewModel = new MapPopupViewModel(_text, OnClick, _animatedText, _appendTextInterval);
            _uiService.OpenView(_mapPopupViewPrefab, _mapPopupViewModel);
            return UniTask.WaitUntil(() => _popupClosed, cancellationToken: cancellationToken);
        }

        private void OnClick()
        {
            _popupClosed = true;
            _uiService.CloseView(_mapPopupViewModel);
        }
    }
}
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
        [SerializeField] private MapPopupView _mapPopupViewPrefab;
        
        [SerializeField] private string _text;
        
        [SerializeField] private bool _animatedText = true;

        [SerializeField] private float _appendTextInterval = 0.05f;

        [SerializeField] private string _buttonText = "Продолжить";
        
        private IUIService _uiService;
        
        private MapPopupViewModel _mapPopupViewModel;

        private bool _popupClosed;
        
        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }
        
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            if (_uiService.TryGetModel<MapPopupViewModel>(out var model))
            {
                _uiService.CloseView(model);
                await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
            }
            _popupClosed = false;
            _mapPopupViewModel = new MapPopupViewModel(_text, OnClick, _animatedText, _appendTextInterval, _buttonText);
            _uiService.OpenView(_mapPopupViewPrefab, _mapPopupViewModel);
            await UniTask.WaitUntil(() => _popupClosed, cancellationToken: cancellationToken);
        }

        private void OnClick()
        {
            _popupClosed = true;
            _uiService.CloseView(_mapPopupViewModel);
        }
    }
}
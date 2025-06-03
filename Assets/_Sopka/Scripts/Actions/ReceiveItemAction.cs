using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.UI;

namespace Sopka
{
    [Serializable]
    public class ReceiveItemAction : AsyncAction
    {
        [SerializeField] private ReceiveItemUIView _viewPrefab;

        [SerializeField] private string _text;
        
        [SerializeField] private Sprite _sprite;
        
        private ReceiveItemUIViewModel _viewModel;
        
        private IUIService _uiService;

        private bool _clicked;
        
        [Inject]
        private void Construct(IUIService uiService)
        {
            _uiService = uiService;
        }
        
        public override async UniTask ExecuteAsync(CancellationToken cancellationToken = default)
        {
            _clicked = false;
            
            _viewModel = new ReceiveItemUIViewModel(_text, _sprite, OnClickContinue);
            _uiService.OpenView(_viewPrefab, _viewModel);

            await UniTask.WaitUntil(() => _clicked, cancellationToken: cancellationToken);

            _clicked = false;
            
            _uiService.CloseView(_viewModel);
        }

        private void OnClickContinue()
        {
            _clicked = true;
        }
    }
}
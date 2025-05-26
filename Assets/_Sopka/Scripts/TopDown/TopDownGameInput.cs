using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Whaledevelop;

namespace Sopka
{
    public class TopDownGameInput
    {
        private const string ACTION_MAP_NAME = "Player";
        private const string ACTION_NAME = "Move";
        
        private readonly InputActionAsset _asset;
        private Vector2 _lastMoveValue;
        private bool _isMovePerforming;

        public Vector2 LastMoveValue => _lastMoveValue;
        public bool IsMovePerforming => _isMovePerforming;

        public event Action<Vector2> OnMovePerformedEvent;
        public event Action<Vector2> OnMoveCanceledEvent;

        private InputActionMap _playerMap;
        
        public TopDownGameInput(InputActionAsset asset)
        {
            _asset = asset;
        }

        public void Enable()
        {
            if (!_asset.enabled)
            {
                _asset.Enable();
            }
            _playerMap ??= _asset.FindActionMap(ACTION_MAP_NAME, true);
            InputSystemUtility.BindAction(_playerMap, ACTION_NAME, OnMovePerformed, OnMoveCanceled);
        }

        public void Disable(bool disableAsset = true)
        {
            if (_playerMap != null)
            {
                InputSystemUtility.UnbindAction(_playerMap, ACTION_NAME, OnMovePerformed, OnMoveCanceled);
            }

            if (disableAsset && _asset.enabled)
            {
                _asset.Disable();
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            _lastMoveValue = context.ReadValue<Vector2>();
            _isMovePerforming = true;

            OnMovePerformedEvent?.Invoke(_lastMoveValue);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            _lastMoveValue = Vector2.zero;
            _isMovePerforming = false;

            OnMoveCanceledEvent?.Invoke(Vector2.zero);
        }
    }
}
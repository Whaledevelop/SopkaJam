using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Whaledevelop;
using Whaledevelop.GameSystems;
using Whaledevelop.Reactive;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/PlayerControllerSystem", fileName = "PlayerControllerSystem")]
    public class TopDownControllerSystem : GameSystem, IFixedUpdate, IUpdate
    {
        [SerializeField]
        private InputActionAsset _inputActionAsset;

        [SerializeField]
        private float _moveSpeed = 3f;

        private IGameModel _gameModel;

        private TopDownGameInput _topDownGameInput;
        private TopDownPlayerView _topDownPlayerView;

        private readonly List<IDisposable> _disposables = new();

        [Inject]
        private void Construct(IGameModel gameModel)
        {
            _gameModel = gameModel;
        }

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _topDownGameInput = new TopDownGameInput(_inputActionAsset);

            _topDownPlayerView = _gameModel.TopDownModel.TopDownPlayerView;
            
            _topDownGameInput.Enable();

            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _disposables.Dispose();
            
            _topDownGameInput.Disable();

            return UniTask.CompletedTask;
        }

        public void OnFixedUpdate()
        {
            var direction = _topDownGameInput.LastMoveValue;
            if (direction == Vector2.zero)
            {
                return;
            }

            var currentPosition = _topDownPlayerView.Rigidbody.position;
            var move = direction.normalized * _moveSpeed * Time.fixedDeltaTime;
            var newPosition = currentPosition + move;

            _topDownPlayerView.Rigidbody.MovePosition(newPosition);
        }

        private Vector2 _prevMoveInput;

        public void OnUpdate()
        {
            var input = _topDownGameInput.LastMoveValue;

            // Debug.Log(input);

           if (input.x != 0 && input.y != 0) // сейчас только 4 анимации, так что для аниамции включаем последнюю нажатую
            {
                if (_prevMoveInput.x != 0)
                {
                    input = new Vector2(0, input.y);
                }
                else if (_prevMoveInput.y != 0)
                {
                    input = new Vector2(input.x, 0);
                } 
                else
                {
                    input = new Vector2(input.x, 0);
                }
            }
            if (input != Vector2.zero)
            {
                _prevMoveInput = input;
            }

            _topDownPlayerView.Animator.SetFloat("MoveX", input.x);
            _topDownPlayerView.Animator.SetFloat("MoveY", input.y);
            _topDownPlayerView.Animator.SetFloat("Speed", input.sqrMagnitude);
        }

    }
}

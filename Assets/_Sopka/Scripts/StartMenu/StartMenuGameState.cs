using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sopka
{
    [CreateAssetMenu(fileName = "StartMenuGameState", menuName = "Sopka/States/StartMenuGameState")]
    public class StartMenuGameState : GameState
    {
        [SerializeField] private StartMenuView _startMenuViewPrefab;

        [SerializeReference] private IAction _startGameAction;
        
        [Inject] private IUIService _uiService;

        [Inject] private IDiContainer _diContainer;
        
        private StartMenuViewModel _startMenuViewModel;
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _startMenuViewModel = new StartMenuViewModel(OnClickStart, OnClickQuit);
            _uiService.OpenView(_startMenuViewPrefab, _startMenuViewModel);
            return UniTask.CompletedTask;
        }

        public override UniTask DisableAsync(CancellationToken cancellationToken)
        {
            if (_startMenuViewModel != null)
            {
                _uiService.CloseView(_startMenuViewModel);
            }
            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            if (_startMenuViewModel != null)
            {
                _uiService.CloseView(_startMenuViewModel);
            }
            _startMenuViewModel = null;
            return UniTask.CompletedTask;
        }

        private void OnClickStart()
        {
            _diContainer.Inject(_startGameAction);
            _startGameAction.Execute();
        }

        private void OnClickQuit()
        {
            ExitGame();
        }
        
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
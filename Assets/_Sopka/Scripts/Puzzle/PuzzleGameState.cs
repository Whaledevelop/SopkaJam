using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;
using Whaledevelop.GameStates;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/PuzzleGameState", fileName = "PuzzleGameState")]
    public class PuzzleGameState : GameState
    {
        [SerializeField] private PuzzleModelInstaller _puzzleModelInstaller;

        [SerializeReference] private IAction _afterPuzzleSolvedAction;
        
        private IDiContainer _diContainer;
        
        [Inject]
        private void Construct(IDiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            // _puzzleModelInstaller.Container = _diContainer;
            // _puzzleModelInstaller.InstallBindings();

            if (_puzzleModelInstaller.Container.TryResolve<IPuzzleModel>(out var puzzleModel))
            {
                puzzleModel.OnPuzzleSolved += OnPuzzleSolved;
            }
            return base.OnInitializeAsync(cancellationToken);
        }

        private void OnPuzzleSolved()
        {
            _diContainer.Inject(_afterPuzzleSolvedAction);
            _afterPuzzleSolvedAction.Execute();
        }
    }
}
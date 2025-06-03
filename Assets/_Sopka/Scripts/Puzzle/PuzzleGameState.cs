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
        
        [Inject] private IDiContainer _diContainer;
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _puzzleModelInstaller.Container = _diContainer;
            _puzzleModelInstaller.InstallBindings();

            if (_puzzleModelInstaller.Container.TryResolve<IPuzzleModel>(out var puzzleModel))
            {
                puzzleModel.OnPuzzleSolved += OnPuzzleSolved;
            }
            return base.OnInitializeAsync(cancellationToken);
        }

        private void OnPuzzleSolved()
        {
            Debug.Log("Puzzle Solved");
            _diContainer.Inject(_afterPuzzleSolvedAction);
            _afterPuzzleSolvedAction.Execute();
        }
    }
}
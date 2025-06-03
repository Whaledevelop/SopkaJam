using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;
using Whaledevelop.GameStates;
using Whaledevelop.GameSystems;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/TopDownGameState", fileName = "TopDownGameState")]
    public class TopDownGameState : GameState
    {
        [SerializeField] 
        private TopDownPlayerView _topDownPlayerViewPrefab;

        [SerializeField] private GameObject _locationPrefab;
        
        private IGameModel _gameModel;
        
        private SceneModel _sceneModel;

        private GameObject _locationInstance;
        
        private TopDownPlayerView _topDownPlayerView;

        [Inject]
        private void Construct(IGameModel gameModel, SceneModel sceneModel)
        {
            _gameModel = gameModel;
            _sceneModel = sceneModel;
        }
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _topDownPlayerView = Instantiate(_topDownPlayerViewPrefab, _sceneModel.PlayerRoot);

            _gameModel.TopDownModel.TopDownPlayerView = _topDownPlayerView;

            _locationInstance = Instantiate(_locationPrefab);
            return UniTask.CompletedTask;
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            Destroy(_topDownPlayerView.gameObject);
            Destroy(_locationInstance);
            return UniTask.CompletedTask;
        }

        public override UniTask EnableAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        public override UniTask DisableAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }
    }
}
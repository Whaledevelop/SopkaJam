using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/States/TopDownGameState", fileName = "TopDownGameState")]
    public class TopDownGameState : GameState
    {
        [SerializeField] 
        private TopDownPlayerView _topDownPlayerViewPrefab;
        
        [SerializeField]
        private TopDownControllerSystem _topDownControllerSystem;

        [SerializeField] private GameObject _locationPrefab;
        
        [Inject] private IGameModel _gameModel;

        [Inject] private IGameSystemsService _gameSystemsService;
        
        [Inject] private SceneModel _sceneModel;

        private GameObject _locationInstance;
        
        private TopDownPlayerView _topDownPlayerView;
        
        protected override async UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _topDownPlayerView = Instantiate(_topDownPlayerViewPrefab, _sceneModel.PlayerRoot);

            _gameModel.TopDownModel.TopDownPlayerView = _topDownPlayerView;

            _locationInstance = Instantiate(_locationPrefab);
            
            await _gameSystemsService.AddSystemAsync(_topDownControllerSystem, cancellationToken);
        }

        protected override async UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            Destroy(_topDownPlayerView.gameObject);
            Destroy(_locationInstance);
            await _gameSystemsService.RemoveSystemAsync(_topDownControllerSystem, cancellationToken);
        }
    }
}
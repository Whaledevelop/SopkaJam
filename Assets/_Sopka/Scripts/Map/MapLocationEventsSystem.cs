using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/MapLocationEventsSystem", fileName = "MapLocationEventsSystem")]
    public class MapLocationEventsSystem : GameSystem
    {
        [SerializeField] private TopDownGameState _topDownGameState;

        [Inject] private IGameModel _gameModel;
        
        [Inject] private IGameStatesService _gameStatesService;
        
        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _gameModel.MapModel.CurrentLocation.Subscribe(OnLocationChanged);
            return base.OnInitializeAsync(cancellationToken);
        }

        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _gameModel.MapModel.CurrentLocation.Unsubscribe(OnLocationChanged);
            return base.OnReleaseAsync(cancellationToken);
        }

        private void OnLocationChanged(MapLocationCode locationCode)
        {
            if (locationCode == MapLocationCode.Test4)
            {
                _gameStatesService.SetStateAsync(_topDownGameState, CancellationToken.None).Forget();
            }
        }
    }
}
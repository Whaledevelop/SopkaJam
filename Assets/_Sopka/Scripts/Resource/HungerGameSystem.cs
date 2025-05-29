using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Whaledevelop;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/HungerGameSystem", fileName = "HungerGameSystem")]
    public class HungerGameSystem : GameSystem, IUpdate
    {
        [Inject] 
        private IGameModel _gameModel;

        private ResourcesModel _resourcesModel;

        private float _hungerTimer;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _resourcesModel = _gameModel.ResourcesModel;

            return base.OnInitializeAsync(cancellationToken);
        }

        public void OnUpdate()
        {
            if (!_resourcesModel.HungerActive.Value)
            {
                return;
            }

            var delta = Time.deltaTime * _resourcesModel.HungerDefaultSpeed.Value;
            _resourcesModel.HungerProgress.Value -= delta;

            if (_resourcesModel.HungerProgress.Value > 0f)
            {
                return;
            }

            if (_resourcesModel.SuppliesCount.Value > 0)
            {
                _resourcesModel.SuppliesCount.Value -= 1;
                _resourcesModel.HungerProgress.Value = 1f;

                return;
            }

            if (_resourcesModel.TeamMembersCount.Value > 0)
            {
                _resourcesModel.TeamMembersCount.Value -= 1;
                _resourcesModel.HungerProgress.Value = 1f;
            }
        }
    }
}
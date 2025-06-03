using System.ComponentModel.Design;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Whaledevelop;
using Whaledevelop.GameSystems;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Systems/HungerGameSystem", fileName = "HungerGameSystem")]
    public class HungerGameSystem : GameSystem, IUpdate
    {
        [SerializeReference] private IAction _deathAction;
        
        [Inject] 
        private IGameModel _gameModel;

        [Inject] private IDiContainer _diContainer;

        private float _hungerTimer;

        private bool _restarting;

        protected override UniTask OnInitializeAsync(CancellationToken cancellationToken)
        {
            _restarting = false;
            return base.OnReleaseAsync(cancellationToken);
        }
        
        protected override UniTask OnReleaseAsync(CancellationToken cancellationToken)
        {
            _restarting = false;
            return base.OnReleaseAsync(cancellationToken);
        }

        public void OnUpdate()
        {
            var resourcesModel = _gameModel.ResourcesModel;
            if (!resourcesModel.HungerActive.Value)
            {
                return;
            }

            var delta = Time.deltaTime * resourcesModel.HungerDefaultSpeed.Value;
            resourcesModel.HungerProgress.Value -= delta;

            if (resourcesModel.HungerProgress.Value > 0f)
            {
                return;
            }

            if (resourcesModel.SuppliesCount.Value > 0)
            {
                resourcesModel.SuppliesCount.Value -= 1;
                resourcesModel.HungerProgress.Value = 1f;

                return;
            }

            if (resourcesModel.TeamMembersCount.Value > 0)
            {
                resourcesModel.TeamMembersCount.Value -= 1;
                resourcesModel.HungerProgress.Value = 1f;
            }
            else if (!_restarting)
            {
                _restarting = true;
                _diContainer.Inject(_deathAction);
                _deathAction.Execute();
            }
        }
    }
}
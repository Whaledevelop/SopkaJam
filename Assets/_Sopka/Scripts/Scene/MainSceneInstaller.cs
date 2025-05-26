using Sopka;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Whaledevelop.DiContainer;

namespace Sopka
{
    public class MainSceneInstaller : MonoBehaviourInstaller
    {
        [SerializeField]
        private Camera _mainCamera;
        
        [SerializeField] 
        private Transform _playerRoot;
        
        public override void InstallBindings()
        {
            var sceneModel = new SceneModel(_playerRoot, _mainCamera);
            Container.Bind(sceneModel);
        }
    }
}
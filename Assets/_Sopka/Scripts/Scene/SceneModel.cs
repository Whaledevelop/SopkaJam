using UnityEngine;

namespace Sopka
{
    public class SceneModel
    {
        public Transform PlayerRoot;

        public Camera MainCamera;

        public SceneModel(Transform playerRoot, Camera mainCamera)
        {
            PlayerRoot = playerRoot;
            MainCamera = mainCamera;
        }
    }
}
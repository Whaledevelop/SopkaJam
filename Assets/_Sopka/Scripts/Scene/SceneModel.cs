using UnityEngine;

namespace Sopka
{
    public class SceneModel
    {
        public Transform PlayerRoot;

        public Camera MainCamera;
        
        public Transform SoundsRoot;

        public SceneModel(Transform playerRoot, Camera mainCamera, Transform soundsRoot)
        {
            PlayerRoot = playerRoot;
            MainCamera = mainCamera;
            SoundsRoot = soundsRoot;
        }
    }
}
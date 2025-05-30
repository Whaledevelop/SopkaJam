#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

namespace Sopka
{
    public static class ComponentFinder
    {
        public static T FindObject<T>() where T : Component
        {
#if UNITY_EDITOR
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                return prefabStage.stageHandle.FindComponentOfType<T>();
            }
#endif
            return Object.FindFirstObjectByType<T>();
        }
    }
}
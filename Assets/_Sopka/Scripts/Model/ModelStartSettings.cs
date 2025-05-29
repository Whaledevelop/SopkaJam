using UnityEngine;
using Sirenix.OdinInspector;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Settings/ModelStartSettings", fileName = "ModelStartSettings")]
    public class ModelStartSettings : ScriptableObject
    {
        [BoxGroup("Начальные ресурсы")]
        [SerializeField]
        private ResourceStartValues _resources;

        public ResourceStartValues Resources => _resources;
    }
}
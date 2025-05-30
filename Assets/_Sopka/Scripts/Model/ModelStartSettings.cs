using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Settings/ModelStartSettings", fileName = "ModelStartSettings")]
    public class ModelStartSettings : ScriptableObject
    {
        [FormerlySerializedAs("_resources")]
        [BoxGroup("Начальные ресурсы")]
        [SerializeField]
        private ResourceModelStartSettings _resourcesModel;

        public ResourceModelStartSettings ResourcesModel => _resourcesModel;
    }
}
using UnityEngine;

namespace Sopka
{
    [CreateAssetMenu(menuName = "Sopka/Settings/PathLineSettings", fileName = "PathLineSettings")]
    public class PathLineSettings : ScriptableObject
    {
        [SerializeField]
        private Color _lineColor = Color.red;

        [SerializeField]
        private float _lineWidth = 0.05f;
        
        [SerializeField]
        private Material _lineMaterial;

        public Color LineColor => _lineColor;

        public float LineWidth => _lineWidth;

        public Material LineMaterial => _lineMaterial;
    }
}
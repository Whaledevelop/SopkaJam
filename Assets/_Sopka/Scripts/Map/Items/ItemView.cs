using UnityEngine;
using UnityEngine.UI;

namespace Sopka
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField] private RectTransform _root;
        
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public Button Button => _button;

        public RectTransform Root => _root;

        public CanvasGroup CanvasGroup => _canvasGroup;
    }
}
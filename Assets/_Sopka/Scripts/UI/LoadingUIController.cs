using TMPro;
using UnityEngine;

namespace Sopka
{
    public class LoadingUIController : MonoBehaviour
    {
        private static LoadingUIController _instance;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void Show(string text)
        {
            if (_instance != null)
            {
                _instance._text.text = text;
                _instance._canvas.enabled = true;
            }
        }

        public static void Hide()
        {
            if (_instance != null)
            {
                _instance._canvas.enabled = false;
            }
        }
    }
}
using System;
using Whaledevelop.UI;

namespace Sopka.UI.MapPopup
{
    public class MapPopupViewModel : IUIViewModel
    {
        public string Text;

        public Action OnClick;

        public bool AnimatedText;

        public float AnimationInterval;

        public MapPopupViewModel(string text, Action onClick, bool animatedText, float animationInterval)
        {
            Text = text;
            OnClick = onClick;
            AnimatedText = animatedText;
            AnimationInterval = animationInterval;
        }
    }
}
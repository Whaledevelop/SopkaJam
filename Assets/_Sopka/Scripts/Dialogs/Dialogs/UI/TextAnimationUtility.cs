using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Whaledevelop
{
    public static class TextAnimationUtility
    {
        public static Sequence BuildTypewriterSequence(TextMeshProUGUI label, string appendText, float baseInterval)
        {
            var sequence = DOTween.Sequence();

            for (var i = 0; i < appendText.Length; i++)
            {
                var letter = appendText[i];

                sequence.AppendCallback(() => label.text += letter);

                if (letter == '.' || letter == '!' || letter == '?')
                {
                    sequence.AppendInterval(baseInterval * 4f);
                }
                else if (letter == ',')
                {
                    sequence.AppendInterval(baseInterval);
                }
                else if (letter != ' ')
                {
                    sequence.AppendInterval(baseInterval);
                }
            }

            return sequence;
        }
    }
}
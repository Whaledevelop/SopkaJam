using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Whaledevelop.Dialogs
{
    public partial class LineDialogNode
    {
        [Serializable]
        private class Data
        {
            [SerializeField]
            [PropertyOrder(1)]
            [ReadOnly]
            [Required]
            private string _speakerId;

            [SerializeField]
            [PropertyOrder(2)]
            [Required]
            [Title("Text", bold: false)]
            [HideLabel]
            [MultiLineProperty(5)]
            private string _text;

            // [SerializeField]
            // private FontStyles _style = FontStyles.Normal;
            
            public string Text
            {
                get => _text;
                set
                {
#if UNITY_EDITOR
                    _text = value;
#else
                    throw new InvalidOperationException();
#endif
                }
            }

            public string SpeakerId
            {
                get => _speakerId;
                set
                {
#if UNITY_EDITOR
                    _speakerId = value;
#else
                    throw new InvalidOperationException();
#endif
                }
            }

#if UNITY_EDITOR
            private bool IsSpeakerSet => string.IsNullOrEmpty(_speakerId);
#endif

#if UNITY_EDITOR

            [NonSerialized]
            private IDialogSettings _dialogSettings;

            public IDialogSettings DialogSettings
            {
                get => _dialogSettings;
                set => _dialogSettings = value;
            }


#endif
            // public FontStyles FontStyle => _style;
            
        }
    }
}
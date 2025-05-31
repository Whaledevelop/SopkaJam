using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Whaledevelop.Dialogs
{
    [CreateAssetMenu(fileName = "SpeakerSettings", menuName = "Whaledevelop/Dialogs/SpeakerSettings")]
    public class SpeakerSettings : ScriptableObject, ISpeakerSettings
    {
        [SerializeField]
        private string _displayName;
        
        [SerializeField]
        private Sprite _icon;
        
        [SerializeField]
        private Color _textColor;

        public string Id => name;
        
        public string DisplayName => _displayName;

        public Sprite Icon => _icon;
        
        public Color NameColor => _textColor;
    }
}
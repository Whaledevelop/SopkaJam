using System;
using UnityEngine;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    [DialogNode("VisualSettings")]
    public class VisualSettingsDialogNode : DialogNode
    {
        [SerializeField] private Sprite _backgroundSprite;

        public Sprite BackgroundSprite => _backgroundSprite;

        protected override DialogNode OnCopy()
        {
            return new VisualSettingsDialogNode
            {
                _backgroundSprite = BackgroundSprite
            };
        }
    }
}
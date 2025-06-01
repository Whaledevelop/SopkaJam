using System;
using Sirenix.OdinInspector;
using Sopka;
using UnityEngine;

namespace Whaledevelop.Dialogs
{
    public partial class VisualsDialogNode
    {
        [Serializable]
        private struct Data
        {
            [SerializeField] 
            private bool _changeBackgroundSprite;
        
            [SerializeField, ShowIf(nameof(_changeBackgroundSprite))]
            private Sprite _backgroundSprite;

            [SerializeField] private bool _changeItemStatus;
        
            [SerializeField, ShowIf(nameof(ChangeItemStatus))]
            private ItemCode _itemCode;

            [SerializeField, ShowIf(nameof(ChangeItemStatus))]
            private bool _itemStatus;

            public Sprite BackgroundSprite => _backgroundSprite;

            public bool ChangeBackgroundSprite => _changeBackgroundSprite;

            public bool ChangeItemStatus => _changeItemStatus;

            public ItemCode ItemCode => _itemCode;

            public bool ItemStatus => _itemStatus;
        }
    }
}
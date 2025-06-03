using System;
using Sirenix.OdinInspector;
using Sopka;
using UnityEngine;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    [DialogNode("Visuals")]
    public partial class VisualsDialogNode : DialogNode, IOneDirectionNode
    {
        [SerializeField]
        private Data _data;
        
        public Sprite BackgroundSprite => _data.BackgroundSprite;

        public bool ChangeBackgroundSprite => _data.ChangeBackgroundSprite;

        public bool ChangeItemStatus => _data.ChangeItemStatus;

        public ItemCode ItemCode => _data.ItemCode;

        public bool ItemStatus => _data.ItemStatus;
        
        protected override DialogNode OnCopy()
        {
            return new VisualsDialogNode
            {
                _data = _data
            };
        }
        [NodeProperty("Next", NodeDirection.Output)]
        public DialogNode NextNode { get; set; }
    }
}
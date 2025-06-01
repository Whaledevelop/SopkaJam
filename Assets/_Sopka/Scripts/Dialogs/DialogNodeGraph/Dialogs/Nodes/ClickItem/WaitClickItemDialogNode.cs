using System;
using UnityEngine;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Sopka
{
    [Serializable]
    [DialogNode("WaitClickItem")]
    public class WaitClickItemDialogNode : DialogNode, IOneDirectionNode
    {
        [SerializeField] private ItemCode _itemCode;

        public ItemCode ItemCode => _itemCode;

        [NodeProperty("Next", NodeDirection.Output)]
        public DialogNode NextNode { get; set; }

        protected override DialogNode OnCopy()
        {
            return new WaitClickItemDialogNode
            {
                _itemCode = _itemCode
            };
        }
    }
}
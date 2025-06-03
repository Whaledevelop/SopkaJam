using System;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    [DialogNode("Start")]
    [Serializable]
    public class StartDialogNode : DialogNode, IOneDirectionNode
    {
        [NodeProperty("Next", NodeDirection.Output)]
        public DialogNode NextNode { get; set; }

        #region DialogNode

        protected override DialogNode OnCopy()
        {
            return new StartDialogNode();
        }

        #endregion
    }
}
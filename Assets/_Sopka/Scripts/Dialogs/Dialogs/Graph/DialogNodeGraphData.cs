using System;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    public class DialogNodeGraphData : NodeGraphData<DialogNode>
    {
        public DialogNode[] CopyNodes()
        {
            var nodes = new DialogNode[Nodes.Length];
            for (var i = 0; i < nodes.Length; i++)
            {
                nodes[i] = Nodes[i].Copy();
            }

            return nodes;
        }
    }
}
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.DialogNodeGraph
{
    public class DialogNodeGraphView : NodeGraphView<DialogNodeView, DialogNodeViewData, DialogNode>
    {
        protected override string NodeGraphStyleSheetName => "DialogNodeGraphBackground";
    }
}
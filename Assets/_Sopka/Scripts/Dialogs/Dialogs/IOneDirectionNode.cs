using Whaledevelop.Dialogs;

namespace Whaledevelop.NodeGraph.Dialogs
{
    public interface IOneDirectionNode
    {
        DialogNode NextNode { get; set; }
    }
}
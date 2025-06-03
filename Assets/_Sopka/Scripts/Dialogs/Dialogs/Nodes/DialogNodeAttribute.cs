using System;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DialogNodeAttribute : NodeAttribute
    {
        public DialogNodeAttribute(string menuTitle, bool hideFromSearch = false)
            : base(menuTitle, hideFromSearch)
        {
        }
    }
}
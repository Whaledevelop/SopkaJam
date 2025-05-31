using System;
using System.Collections.Generic;
using System.Linq;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;
using Whaledevelop.Utilities;

namespace Whaledevelop.DialogNodeGraph
{
    internal sealed class DialogNodeSearchWindowProvider : NodeSearchWindowProvider
    {
        protected override IOrderedEnumerable<KeyValuePair<Type, string>> GetSortedNodes()
        {
            
            return DialogNodesUtility.NODES
                .Where(pair => typeof(DialogNode).IsAssignableFrom(pair.Key) && !pair.Value.HideFromSearch)
                .Select(pair => new KeyValuePair<Type, string>(pair.Key, pair.Value.MenuTitle))
                .OrderBy(pair => pair.Value.Split('/').Length)
                .ThenBy(pair => pair.Value);
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    public interface IDialogSettings : IGraphDataSettings<IDialogSettings, DialogNodeGraphData, DialogNode>
    {
        string Id { get; }
        ISpeakerSettings[] SpeakersSettings { get; }
        
        IAction AfterDialogAction { get; }
        
        Sprite StartBackground { get; }
    }
}
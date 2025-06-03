using UnityEngine;

namespace Whaledevelop.Dialogs
{
    public interface ISpeakerSettings
    {
        string Id { get; }
        string DisplayName { get; }
        
        Sprite Icon { get; }
        
        Color NameColor { get; }
    }
}
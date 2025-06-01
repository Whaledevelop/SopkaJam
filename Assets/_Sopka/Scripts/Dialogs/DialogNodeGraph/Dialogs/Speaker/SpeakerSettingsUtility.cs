using UnityEngine;

namespace Whaledevelop.Dialogs
{
    public static class SpeakerSettingsUtility
    {
        public static string GetNameText(this ISpeakerSettings speakerSettings)
        {
            if (string.IsNullOrEmpty(speakerSettings.DisplayName))
            {
                return string.Empty;
            }
            var displayName = speakerSettings.DisplayName;
            var nameColor = ColorUtility.ToHtmlStringRGB(speakerSettings.NameColor);
            return $"<color=#{nameColor}>{displayName}</color>";
        }
    }
}
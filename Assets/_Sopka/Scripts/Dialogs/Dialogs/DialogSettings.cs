using System.Collections.Generic;
using UnityEngine;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    [CreateAssetMenu(fileName = "DialogSettings", menuName = "Whaledevelop/Dialogs/DialogSettings")]
    public class DialogSettings : GraphDataSettings<IDialogSettings, DialogNodeGraphData, DialogNode>, IDialogSettings
    {
        [SerializeField]
        private SpeakerSettings[] _speakers;
        
        [SerializeField]
        private Sprite _startBackground;
        
        // Костыль
        [SerializeReference]
        private IAction _afterDialogAction;
        
#if UNITY_EDITOR

        public SpeakerSettings[] Speakers
        {
            set => _speakers = value;
        }
#endif

        #region IDialogSettings

        public string Id => name;

        ISpeakerSettings[] IDialogSettings.SpeakersSettings => _speakers;

        // ReSharper disable once ConvertToAutoProperty
        public IAction AfterDialogAction => _afterDialogAction;

        public Sprite StartBackground => _startBackground;

        #endregion
    }
}
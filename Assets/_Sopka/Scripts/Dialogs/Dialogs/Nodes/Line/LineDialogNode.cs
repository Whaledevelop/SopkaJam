using System;
using TMPro;
using UnityEngine;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    [DialogNode("Line")]
    public partial class LineDialogNode : DialogNode, IOneDirectionNode
    {
        [SerializeField]
        private Data _data = new();

        public string Text
        {
            get => _data.Text;
#if UNITY_EDITOR
            set => _data.Text = value;
#endif
        }

        public string SpeakerId
        {
            get => _data.SpeakerId;
#if UNITY_EDITOR
            set => _data.SpeakerId = value;
#endif
        }

        // public FontStyles FontStyle => _data.FontStyle;

        [NodeProperty("Next", NodeDirection.Output)]
        public DialogNode NextNode { get; set; }

#if UNITY_EDITOR

        protected override void OnDialogSettingsSet()
        {
            _data.DialogSettings = DialogSettings;
        }

#endif

        protected override DialogNode OnCopy()
        {
            return new LineDialogNode
            {
                _data = _data
            };
        }
    }
}
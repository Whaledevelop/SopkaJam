using System;
using UnityEngine;
using Whaledevelop.Dialogs.UI;
using Whaledevelop.UI;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    public class DialogUISettings
    {
        [SerializeField]
        private DialogView _dialogView;

        [SerializeField]
        private float _textAppendInterval = 0.03f;

        [SerializeField]
        private float _startDialogDelay = 0.5f;

        [SerializeField]
        private float _nextLineDelay = 1.0f;

        [SerializeField]
        private float _clearOptionsDelay = 0.0f;

        [SerializeField] private SpeakerSettings _narratorSettings;
        
        // TODO костыли уже пошли
        [SerializeReference] private IAction _hideSnakeAction;

        public DialogView DialogView => _dialogView;

        public float TextAppendInterval => _textAppendInterval;

        public float StartDialogDelay => _startDialogDelay;

        public float NextLineDelay => _nextLineDelay;

        public float ClearOptionsDelay => _clearOptionsDelay;

        public SpeakerSettings NarratorSettings => _narratorSettings;

        public IAction HideSnakeAction => _hideSnakeAction;
    }
}
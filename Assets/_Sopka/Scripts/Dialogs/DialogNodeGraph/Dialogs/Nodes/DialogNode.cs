using System;
using Whaledevelop.NodeGraph;

namespace Whaledevelop.Dialogs
{
    [Serializable]
    public abstract class DialogNode : BaseNode
    {
        [NodeProperty("Start", NodeDirection.Input, NodePortCapacity.Multi)]
        public DialogNode StartNode { get; set; }

        internal DialogNodeGraphProcessor GraphProcessor { get; set; }

        public virtual string NodeId => Guid;

        protected abstract DialogNode OnCopy();

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        public void Start()
        {
            OnStart();
        }

        public void Stop()
        {
            OnStop();
        }

        public DialogNode Copy()
        {
            var copy = OnCopy();
            copy.Guid = Guid;
            return copy;
        }

        public override string ToString()
        {
            return $"{NodeId}";
        }

#if UNITY_EDITOR

        [NonSerialized]
        private IDialogSettings _dialogSettings;

        public IDialogSettings DialogSettings
        {
            get => _dialogSettings;
            set
            {
                _dialogSettings = value;
                OnDialogSettingsSet();
            }
        }

        protected virtual void OnDialogSettingsSet()
        {
        }

#endif
    }
}
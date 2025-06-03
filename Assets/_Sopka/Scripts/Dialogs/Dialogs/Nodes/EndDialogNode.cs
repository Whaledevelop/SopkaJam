using System;
using UnityEngine;
using Whaledevelop.NodeGraph;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.Dialogs
{
    [DialogNode("End")]
    [Serializable]
    public class EndDialogNode : DialogNode, IOneDirectionNode
    {
        [SerializeField]
        private string _finishBranchId;

        public string FinishBranchId => _finishBranchId;

        public DialogNode NextNode
        {
            get => null;
            set => throw new NotImplementedException();
        }

        protected override DialogNode OnCopy()
        {
            return new EndDialogNode
            {
                _finishBranchId = _finishBranchId
            };
        }
    }
}
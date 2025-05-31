using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Whaledevelop.Dialogs.UI;
using Whaledevelop.UI;

namespace Whaledevelop.Dialogs
{
    public class DialogProcessor : IDisposable
    {
        [ShowInInspector]
        private readonly IDialogSettings _dialogSettings;
        private readonly DialogUISettings _uISettings;

        [ShowInInspector]
        private DialogProfile _dialogProfile;

        [Inject]
        private IDiContainer _diContainer;

        [Inject]
        private IUIService _uiService;
        
        public event Action<string> OnEndNode;
        public event Action<int> OnChooseOption;
        public event Action OnEndDialog;

        private DialogView _dialogView;
        private DialogNode _currentNode;
        private DialogViewModel _dialogViewModel;
        
        public DialogProcessor(IDialogSettings dialogSettings, DialogUISettings uiSettings)
        {
            _dialogSettings = dialogSettings;
            _uISettings = uiSettings;
        }

        public void Dispose()
        {
            OnEndNode = null;
            OnChooseOption = null;

            OnEndDialog?.Invoke();
            OnEndDialog = null;
        }

        private void ProcessEndNode()
        {
            OnEndNode?.Invoke(_dialogProfile.NodeId);
        }

        private void EndDialog()
        {
            if (_dialogViewModel != null)
            {
                _uiService.CloseView(_dialogViewModel);
            }
            Dispose();
        }
        
        private void StartDialog()
        {
            var sprites = _dialogSettings.SpeakersSettings.Select(speaker => speaker.Icon).ToArray();
            _dialogViewModel = new DialogViewModel(sprites, _dialogSettings.StartBackground, OnClickNext, OnOptionSelected, _uISettings.TextAppendInterval, EndDialog);
            _uiService.OpenView(_uISettings.DialogView, _dialogViewModel);
    
            UniTaskUtility.ExecuteAfterSeconds(_uISettings.StartDialogDelay, ProcessEndNode, CancellationToken.None);
        }

        // private void SkipDialog()
        // {
        //     
        // }

        private void OnClickNext()
        {
            if (_currentNode is LineDialogNode)
            {
                UniTaskUtility.ExecuteAfterSeconds(_uISettings.NextLineDelay, ProcessEndNode, CancellationToken.None);
            }
        }

        private void OnOptionSelected(int optionIndex)
        {
            if (_currentNode is BranchingDialogNode)
            {
                OnChooseOption?.Invoke(optionIndex);
                UniTaskUtility.ExecuteAfterSeconds(_uISettings.ClearOptionsDelay, _dialogViewModel.Options.Clear, CancellationToken.None);
            }
        }
        
        public void ProcessDialogNode(DialogProfile dialogProfile)
        {
            _dialogProfile = dialogProfile;
            if (_dialogSettings.Id != _dialogProfile.DialogId)
            {
                return;
            }
            _currentNode = _dialogSettings.Graph.Nodes.FirstOrDefault(node => node.NodeId == _dialogProfile.NodeId);
            if (_currentNode == null)
            {
                return;
            }

            switch (_currentNode)
            {
                case StartDialogNode:
                {
                    StartDialog();
                    break;
                }
                case EndDialogNode:
                {
                    ProcessEndNode();
                    EndDialog();
                    
                    break;
                }
                case LineDialogNode lineNode:
                {
                    var nameText = GetSpeakerNameText(lineNode);
                    _dialogViewModel.DialogLine.Value = (nameText, lineNode.Text);
                    
                    break;
                }
                case BranchingDialogNode branchingDialogNode:
                {
                    _dialogViewModel.Options.Set(branchingDialogNode.Options);
                    
                    break;
                }
                case ActionDialogNode actionDialogNode:
                {
                    ProcessActionNode(actionDialogNode).Forget();
                    break;
                }
                case VisualSettingsDialogNode visualSettingsDialogNode:
                {
                    _dialogViewModel.BackgroundSprite.Value = visualSettingsDialogNode.BackgroundSprite;
                    ProcessEndNode();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetSpeakerNameText(LineDialogNode lineNode)
        {
            var speaker = _dialogSettings.SpeakersSettings.FirstOrDefault(s => s.Id == lineNode.SpeakerId);
            if (speaker == null || string.IsNullOrEmpty(speaker.DisplayName))
            {
                return string.Empty;
            }
            var displayName = speaker.DisplayName;
            var nameColor = ColorUtility.ToHtmlStringRGB(speaker.NameColor);
            return $"<color=#{nameColor}>{displayName}</color>";
        }

        private async UniTask ProcessActionNode(ActionDialogNode actionDialogNode)
        {
            await actionDialogNode.ExecuteAsync(_diContainer);
            ProcessEndNode();
        }
    }
}
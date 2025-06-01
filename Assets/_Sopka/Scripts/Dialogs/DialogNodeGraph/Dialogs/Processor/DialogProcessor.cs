using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sopka;
using TMPro;
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
            // var sprites = _dialogSettings.SpeakersSettings.Select(speaker => speaker.Icon).ToArray();
            _dialogViewModel = new DialogViewModel(_dialogSettings.StartBackground,
                OnClickNext, OnOptionSelected, _uISettings.TextAppendInterval, EndDialog, OnClickItem);
            _uiService.OpenView(_uISettings.DialogView, _dialogViewModel);
    
            UniTaskUtility.ExecuteAfterSeconds(_uISettings.StartDialogDelay, ProcessEndNode, CancellationToken.None);
        }

        private void OnClickItem(ItemCode itemCode)
        {
            if (_dialogViewModel.ItemsStatuses.ContainsKey(itemCode))
            {
                _dialogViewModel.ItemsStatuses[itemCode] = false;
                
                switch (itemCode)
                {
                    case ItemCode.Snake:
                        _diContainer.Inject(_uISettings.HideSnakeAction);
                        _uISettings.HideSnakeAction.Execute();
                        break;
                    case ItemCode.Fish:
                        _diContainer.Inject(_uISettings.HideFishAction);
                        _uISettings.HideFishAction.Execute();
                        break;
                    case ItemCode.Tree:
                        _diContainer.Inject(_uISettings.HideTreeAction);
                        _uISettings.HideTreeAction.Execute();
                        break;
                    case ItemCode.Buben:
                        _diContainer.Inject(_uISettings.HideBubenAction);
                        _uISettings.HideBubenAction.Execute();
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemCode), itemCode, null);
                }
            }
        }

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
                _dialogViewModel.Options.Clear();
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
                    var speakerSettings = _dialogSettings.SpeakersSettings.FirstOrDefault(s => s.Id == lineNode.SpeakerId);
                    if (speakerSettings == null)
                    {
                        Debug.LogError($"Speaker with id {lineNode.SpeakerId} not found");
                        break;
                    }
                    var nameText = speakerSettings.GetNameText();
                    _dialogViewModel.SpeakerName.Value = nameText;
                    _dialogViewModel.SpeakerSprite.Value = speakerSettings.Icon;
                    var isNarrator = lineNode.SpeakerId == _uISettings.NarratorSettings.Id;
                    _dialogViewModel.FontStyle.Value = isNarrator ? FontStyles.Italic : FontStyles.Normal;
                    _dialogViewModel.MainText.Value = lineNode.Text;

                    
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
                case VisualsDialogNode visualsDialogNode:
                {
                    if (visualsDialogNode.ChangeBackgroundSprite)
                    {
                        _dialogViewModel.BackgroundSprite.Value = visualsDialogNode.BackgroundSprite;
                    }
                    else if (visualsDialogNode.ChangeItemStatus)
                    {
                        _dialogViewModel.ItemsStatuses.Set(visualsDialogNode.ItemCode, visualsDialogNode.ItemStatus);
                    }
                    ProcessEndNode();
                    break;
                }
                case WaitClickItemDialogNode waitClickItemDialogNode :
                {
                    _dialogViewModel.Options.Clear(); // костыль чтобы кнопку убрать
                    if (!_dialogViewModel.ItemsStatuses.TryGetValue(waitClickItemDialogNode.ItemCode, out var status))
                    {
                        ProcessEndNode();
                        break;
                    }
                    if (status)
                    {
                        WaitItemStatusAsync(waitClickItemDialogNode.ItemCode, false, CancellationToken.None).Forget();
                    }
                    else
                    {
                        ProcessEndNode();
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask WaitItemStatusAsync(ItemCode itemCode, bool waitStatus, CancellationToken cancellationToken)
        {
            await UniTask.WaitUntil(() => _dialogViewModel.ItemsStatuses.TryGetValue(itemCode, out var status) && waitStatus == status, 
                cancellationToken: cancellationToken);
            ProcessEndNode();
        }

        private async UniTask ProcessActionNode(ActionDialogNode actionDialogNode)
        {
            await actionDialogNode.ExecuteAsync(_diContainer);
            ProcessEndNode();
        }
    }
}
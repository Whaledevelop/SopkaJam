using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;

#if UNITY_EDITOR
namespace Whaledevelop.DialogNodeGraph
{
    [NodeEditor(typeof(DialogNode))]
    public class DialogNodeView : NodeView<DialogNodeViewData, DialogNode>
    {
        private IDialogSettings _dialogSettings;
        private VisualElement _extensionContainerContent;
        private TextField _keyTextField;
        private Action<string> _recordObjectForUndo;
        private DropdownField _speakerField;

        protected override string PortViewStyleSheetName => "DialogPortView";

        protected override string LastTitle => string.Empty;

        protected override void OnAddToSelection()
        {
            UpdateValues();
            UpdateDialogSettings();
        }

        protected override void OnRefresh()
        {
            UpdateValues();
            UpdateDialogSettings();
        }

        public void Initialize(IDialogSettings dialogSettings, Action<string> recordObjectForUndo)
        {
            _dialogSettings = dialogSettings;
            _recordObjectForUndo = recordObjectForUndo;

            Initialize();
            CreateExtensionContainerContent();
        }

        #region Extensions container view

        private List<string> GetSpeakerDropdownChoices()
        {
            return _dialogSettings?.SpeakersSettings == null
                ? new()
                : _dialogSettings.SpeakersSettings.Select(speaker => speaker.Id).ToList();
        }

        private void CreateSpeakerDropdown(LineDialogNode lineNode)
        {
            var choices = GetSpeakerDropdownChoices();
            _speakerField = new("Speaker : ")
            {
                choices = choices
            };
            if (string.IsNullOrEmpty(lineNode.SpeakerId) && choices.Count > 0)
            {
                lineNode.SpeakerId = choices[0];
            }
            _speakerField.RegisterCallback<ChangeEvent<string>>(OnValueChanged);
            _extensionContainerContent.Add(_speakerField);

            void OnValueChanged(ChangeEvent<string> evt)
            {
                _recordObjectForUndo?.Invoke($"Change {name}");
                lineNode.SpeakerId = evt.newValue;
            }
        }

        private void CreateExtensionContainerContent()
        {
            _extensionContainerContent = new();
            if (Data.Value is LineDialogNode lineNode)
            {
                CreateSpeakerDataView(lineNode);
            }

            if (Data.Value is ActionDialogNode actionNode)
            {
                _keyTextField = new("Description")
                {
                    value = actionNode.Description,
                    // isReadOnly = true
                };
                _keyTextField.AddToClassList("DialogNodeExtensionsContainer");
                _extensionContainerContent.Add(_keyTextField);
            }
            if (_extensionContainerContent.childCount > 0)
            {
                _extensionContainerContent.AddToClassList("DialogNodeExtensionsContainer");
                extensionContainer.Add(_extensionContainerContent);
            }
            RefreshExpandedState();
        }

        private void CreateSpeakerDataView(LineDialogNode node)
        {
            CreateSpeakerDropdown(node);
            _keyTextField = new("Text")
            {
                value = node.Text
            };
            _keyTextField.RegisterCallback<ChangeEvent<string>>(OnValueChanged);
            _keyTextField.AddToClassList("DialogNodeExtensionsContainer");

            _extensionContainerContent.Add(_keyTextField);

            void OnValueChanged(ChangeEvent<string> evt)
            {
                _recordObjectForUndo?.Invoke($"Change {name}");
                node.Text = evt.newValue;
            }
        }

        #endregion

        #region Update values

        private void UpdateValues()
        {
            if (_dialogSettings == null)
            {
                return;
            }
            if (Data.Value is LineDialogNode lineNode)
            {
                UpdateLineNode(lineNode);
            }
        }

        private void UpdateDialogSettings()
        {
            Data.Value.DialogSettings = _dialogSettings;
        }

        private void UpdateLineNode(LineDialogNode node)
        {
            if (_keyTextField == null)
            {
                return;
            }
            if (_keyTextField.value != node.Text)
            {
                _keyTextField.value = node.Text;
            }
            if (_speakerField == null)
            {
                if (_dialogSettings is not { SpeakersSettings: not null })
                {
                    return;
                }
                CreateSpeakerDropdown(node);
            }
            if (_speakerField == null)
            {
                return;
            }
            _speakerField.choices = GetSpeakerDropdownChoices();
            _speakerField.value = node.SpeakerId;
            ResetTitle();
        }

        #endregion
    }
}

#endif
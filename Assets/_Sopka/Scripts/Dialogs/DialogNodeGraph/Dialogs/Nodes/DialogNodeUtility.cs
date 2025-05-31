using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;
using Whaledevelop.DialogNodeGraph;
using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;
using Whaledevelop.Scopes;

namespace Whaledevelop.Utilities
{
    public static class DialogNodesUtility
    {
        public static readonly Dictionary<Type, DialogNodeAttribute> NODES = new();
        private static readonly Dictionary<Type, Type> NODE_VIEWS = new();

        static DialogNodesUtility()
        {
            using (HashSetScope<Assembly>.Create(out var assemblies))
            {
                assemblies.Add(Assembly.GetAssembly(typeof(DialogNodeView)));
                assemblies.Add(Assembly.GetAssembly(typeof(DialogNode)));

                foreach (var assembly in assemblies)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        var nodeAttribute = type.GetCustomAttribute<DialogNodeAttribute>();
                        if (nodeAttribute != null)
                        {
                            NODES[type] = nodeAttribute;
                        }

                        var nodeEditorAttribute = type.GetCustomAttribute<NodeEditorAttribute>();
                        if (nodeEditorAttribute == null)
                        {
                            continue;
                        }

                        NODE_VIEWS[nodeEditorAttribute.NodeType] = type;
                    }
                }
            }
        }

        private static Type GetNodeViewType(Type nodeType)
        {
            if (!NODE_VIEWS.TryGetValue(nodeType, out var nodeViewType))
            {
                nodeViewType = NODE_VIEWS[typeof(DialogNode)];
            }
            if (nodeViewType.IsGenericType)
            {
                nodeViewType = nodeViewType.MakeGenericType(nodeType);
            }
            return nodeViewType;
        }

        public static DialogNodeView CreateNodeView(DialogNodeViewData viewData, IDialogSettings dialogSettings,
            Action<string> recordObjectForUndo)
        {
            var nodeViewType = GetNodeViewType(viewData.GetType());
            var nodeView = (DialogNodeView)Activator.CreateInstance(nodeViewType);

            nodeView.Data = viewData;
            nodeView.SetPosition(viewData.Position);
            nodeView.Initialize(dialogSettings, recordObjectForUndo);

            return nodeView;
        }

        public static DropdownField CreateDropdownFromEnum<T>(string label, Action<T> changeDelegate)
            where T : struct, Enum
        {
            var choices = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(type => type.ToString())
                .ToList();
            var dropdownField = new DropdownField(label)
            {
                choices = choices
            };

            dropdownField.RegisterCallback<ChangeEvent<string>>(OnValueChanged);

            return dropdownField;

            void OnValueChanged(ChangeEvent<string> evt)
            {
                if (Enum.TryParse<T>(evt.newValue, out var branchingType))
                {
                    changeDelegate.Invoke(branchingType);
                }
            }
        }

        public static void WarmUp()
        {
        }
    }
}
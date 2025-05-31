using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;
using Whaledevelop.Utilities;
using UnityEditor;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.DialogNodeGraph
{
    public sealed class DialogNodeGraphWindow : NodeGraphWindow<DialogNodeGraphView, DialogNodeView, DialogNodeGraphEditorData,
        DialogNodeViewData, DialogNode>
    {
        public const string WINDOW_NAME = "Dialog Nodes Graph";

        private DialogSettings _dialogSettings;

        protected override string DirectoryPath => DialogAssetsPaths.DIALOG_NODE_GRAPH_PATH;

        protected override string NewFileName => "New dialog";

        protected override DialogNodeView CreateNodeView(DialogNodeViewData nodeViewData)
        {
            ResetDialogSettings();

            return DialogNodesUtility.CreateNodeView(nodeViewData, _dialogSettings, RecordObjectForUndo);
        }

        protected override NodeSearchWindowProvider CreateNodeSearchWindowProvider()
        {
            
            return CreateInstance<DialogNodeSearchWindowProvider>();
        }

        protected override void OnWindowEnabled()
        {
            DialogNodesUtility.WarmUp();
        }

        protected override void OnGraphLoaded()
        {
            ResetDialogSettings();
        }

        protected override void OnGraphSaved(string path)
        {
            var graphData = AssetDatabase.LoadAssetAtPath<DialogNodeGraphEditorData>(path);

            graphData.ApplyToSettings();
            EditorUtility.DisplayDialog("Save graph", "Graph saved and applied to settings", "Ok");
        }

        private void OnShowSettingsClick()
        {
            ResetDialogSettings();
            Selection.activeObject = _dialogSettings;
        }

        private void ResetDialogSettings()
        {
            if (GraphData == null || _dialogSettings != null && _dialogSettings.name == GraphData.name)
            {
                return;
            }

            _dialogSettings = EditorAssetUtility.GetScriptableObjectOrCreate<DialogSettings>(GraphData.name, DialogAssetsPaths.DIALOGS_PATH);
        }

        [MenuItem("Whaledevelop/DialogNodesGraph")]
        public static void OpenGameNodesGraph()
        {
            GetWindow<DialogNodeGraphWindow>(WINDOW_NAME).Show();
        }
    }
}
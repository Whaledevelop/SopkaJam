using Whaledevelop.Dialogs;
using Whaledevelop.NodeGraph;
using Whaledevelop.Utilities;
using UnityEditor;
using Whaledevelop.NodeGraph.Dialogs;

namespace Whaledevelop.DialogNodeGraph
{
    public class DialogNodeGraphEditorData : NodeGraphEditorData<DialogNodeViewData, DialogNode>
    {
        public override INodeGraphSelection NodeGraphSelection => DialogNodeGraphSelection.instance;

        public override void OpenInGraph()
        {
            var graphWindow = EditorWindow.GetWindow<DialogNodeGraphWindow>(DialogNodeGraphWindow.WINDOW_NAME);
            graphWindow.ShowGraph(this);
        }

        protected override void OpenGeneratedFile()
        {
            var settings = EditorAssetUtility.GetScriptableObjectOrCreate<DialogSettings>(name, DialogAssetsPaths.DIALOGS_PATH);
            Selection.activeObject = settings;
            EditorGUIUtility.PingObject(settings);
        }

        public override void ApplyToSettings()
        {
            var settings = EditorAssetUtility.GetScriptableObjectOrCreate<DialogSettings>(name, DialogAssetsPaths.DIALOGS_PATH);

            TransformToGraphData(settings);
        }
    }
}
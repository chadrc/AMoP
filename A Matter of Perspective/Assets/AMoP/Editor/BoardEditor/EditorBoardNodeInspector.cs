using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EditorBoardNodeBehavior))]
public class EditorBoardNodeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        var node = (EditorBoardNodeBehavior)target;

        AMoPEditorUtils.EditBoardNodeDataHeader();
        AMoPEditorUtils.EditBoardNodeData("Node: ", node.data);
    }
}

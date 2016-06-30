using UnityEngine;
using UnityEditor;
using System.Collections;

public static class AMoPEditorUtils
{
    public static void EditBoardNodeDataHeader()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Index", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Position", GUILayout.Width(130f));
        EditorGUILayout.LabelField("Energy", GUILayout.Width(60f));
        EditorGUILayout.LabelField("Type", GUILayout.Width(70f));
        EditorGUILayout.LabelField("Affiliation", GUILayout.Width(70f));

        EditorGUILayout.EndHorizontal();
    }

    public static bool EditBoardNodeData(string label, BoardNodeData node)
    {
        bool delete = false;
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField(label, GUILayout.Width(40f));
        string[] posOptions = { "0", "1", "2", "3", "4", "5" };
        int[] posOptionsVals = { 0, 1, 2, 3, 4, 5 };
        int x = EditorGUILayout.IntPopup((int)node.Position.x, posOptions, posOptionsVals, GUILayout.Width(40f));
        int y = EditorGUILayout.IntPopup((int)node.Position.y, posOptions, posOptionsVals, GUILayout.Width(40f));
        int z = EditorGUILayout.IntPopup((int)node.Position.z, posOptions, posOptionsVals, GUILayout.Width(40f));
        node.Position = new Vector3(x, y, z);
        node.StartingEnergy = EditorGUILayout.FloatField(node.StartingEnergy, GUILayout.Width(60f));
        node.Type = (BoardNodeType)EditorGUILayout.EnumPopup(node.Type, GUILayout.Width(70f));
        node.Affiliation = (BoardNodeAffiliation)EditorGUILayout.EnumPopup(node.Affiliation, GUILayout.Width(70f));
        var oldClr = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("X", GUILayout.Height(14f), GUILayout.Width(20f)))
        {
            delete = true;
        }
        GUI.backgroundColor = oldClr;
        EditorGUILayout.EndHorizontal();
        return delete;
    }
}

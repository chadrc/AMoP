using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(EditorBoardNodeBehavior))]
public class EditorBoardNodeInspector : Editor
{
	private const float controlsHeight = 150f;
	private const float controlsWidth = 300f;

	private EditorBoardNodeBehavior node;

	void OnEnable()
	{
		node = (EditorBoardNodeBehavior)target;
	}

	void OnFocus()
	{
		node = (EditorBoardNodeBehavior)target;
	}

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Node: " + node.NodeIndex);
        if (node == null || node.Data == null)
        {
            EditorGUILayout.LabelField("No Data");
        }
        else
        {
            //EditorGUI.BeginChangeCheck();
            //AMoPEditorUtils.EditBoardNodeDataHeader();
            //bool delete = AMoPEditorUtils.EditBoardNodeData("Node: ", node.Data,);

            //if (EditorGUI.EndChangeCheck() || delete)
            //{
            //    node.InspectorEdited(delete);
            //}
        }
    }

	private void OnSceneGUI()
	{
		if (node == null || node.Data == null)
		{
			return;
		}

        Handles.BeginGUI();

        var sceneViewRect = Camera.current.pixelRect;
        var controlRect = new Rect(0, sceneViewRect.height - controlsHeight, controlsWidth, controlsHeight);
        var backClr = new Color(1f, 1f, 1f, .5f);
        var style = new GUIStyle();
        style.normal.background = AMoPEditorUtils.MakeTex((int)controlRect.width, (int)controlRect.height, backClr);
        GUILayout.BeginArea(controlRect, style);

        GUILayout.Label("Controls [" + node.name + "]");

        GUILayout.BeginHorizontal(GUILayout.Width(controlsWidth));
        GUILayout.BeginVertical(GUILayout.Width(100f));

        Vector3 pos;
        pos.x = IncDecControl("Pos X: ", node.Data.Position.x);
        pos.y = IncDecControl("Pos Y: ", node.Data.Position.y);
        pos.z = IncDecControl("Pos Z: ", node.Data.Position.z);
        node.Data.Position = pos;

        GUILayout.EndVertical();

        GUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Energy:", GUILayout.Width(70f));
        node.Data.StartingEnergy = EditorGUILayout.IntField((int)node.Data.StartingEnergy, GUILayout.Width(50f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Type:", GUILayout.Width(70f));
        node.Data.Type = (BoardNodeType)EditorGUILayout.EnumPopup(node.Data.Type, GUILayout.Width(70f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Affiliation:", GUILayout.Width(70f));
        node.Data.Affiliation = (BoardNodeAffiliation)EditorGUILayout.EnumPopup(node.Data.Affiliation, GUILayout.Width(70f));
        EditorGUILayout.EndHorizontal();

        bool delete = false;
        var oldClr = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Delete Node", GUILayout.Width(100f)))
        {
            delete = true;
        }

        GUI.backgroundColor = oldClr;

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.EndArea();

        if (GUI.changed || delete)
        {
            node.InspectorEdited(delete);
        }

        Handles.EndGUI();

        Handles.DrawWireDisc(node.transform.position, Vector3.back, .5f);
    }

    private float IncDecControl(String label, float value)
	{
		GUILayout.BeginVertical ();
		GUILayout.Label (label + value);

		GUILayout.BeginHorizontal ();
		if (GUILayout.Button ("<"))
		{
			value--;
		}

		if (GUILayout.Button (">"))
		{
			value++;
		}
		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();

		return value;
	}
}

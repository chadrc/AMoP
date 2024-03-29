﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public static class AMoPEditorUtils
{
	private const float editBoardNodeData_LabelWidth = 40f;
	private const float editBoardNodeData_PosElementWidth = 40f;
	private const float editBoardNodeData_StartingEnergyWidth = 60f;
	private const float editBoardNodeData_TypeWidth = 70f;
	private const float editBoardNodeData_AffiliationWidth = 70f;
	private const float editBoardNodeData_DeleteButtonWidth = 20f;

	public static float EditBoardNodeDataWidth 
	{
		get {
			return editBoardNodeData_LabelWidth + 
				editBoardNodeData_PosElementWidth*3 +
				editBoardNodeData_StartingEnergyWidth +
				editBoardNodeData_TypeWidth +
				editBoardNodeData_AffiliationWidth +
				editBoardNodeData_DeleteButtonWidth
				;
		}
	}

    public static void EditBoardNodeDataHeader()
    {
        EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField("Index", GUILayout.Width(editBoardNodeData_LabelWidth));
		EditorGUILayout.LabelField("Position", GUILayout.Width(editBoardNodeData_PosElementWidth*3 + 10f));
		EditorGUILayout.LabelField("Energy", GUILayout.Width(editBoardNodeData_StartingEnergyWidth));
		EditorGUILayout.LabelField("Type", GUILayout.Width(editBoardNodeData_TypeWidth));
		EditorGUILayout.LabelField("Affiliation", GUILayout.Width(editBoardNodeData_AffiliationWidth));

        EditorGUILayout.EndHorizontal();
    }

    public static bool EditBoardNodeData(string label, BoardNodeData node, int boardSize)
    {
        bool delete = false;
        EditorGUILayout.BeginHorizontal();

		EditorGUILayout.LabelField(label, GUILayout.Width(editBoardNodeData_LabelWidth));
        string[] posOptions = new string[boardSize];
        int[] posOptionsVals = new int[boardSize];

        for (int i=0; i<boardSize; i++)
        {
            posOptions[i] = i.ToString();
            posOptionsVals[i] = i;
        }

		int x = EditorGUILayout.IntPopup((int)node.Position.x, posOptions, posOptionsVals, GUILayout.Width(editBoardNodeData_PosElementWidth));
		int y = EditorGUILayout.IntPopup((int)node.Position.y, posOptions, posOptionsVals, GUILayout.Width(editBoardNodeData_PosElementWidth));
		int z = EditorGUILayout.IntPopup((int)node.Position.z, posOptions, posOptionsVals, GUILayout.Width(editBoardNodeData_PosElementWidth));
        node.Position = new Vector3(x, y, z);
		node.StartingEnergy = EditorGUILayout.FloatField(node.StartingEnergy, GUILayout.Width(editBoardNodeData_StartingEnergyWidth));
		node.Type = (BoardNodeType)EditorGUILayout.EnumPopup(node.Type, GUILayout.Width(editBoardNodeData_TypeWidth));
		node.Affiliation = (BoardNodeAffiliation)EditorGUILayout.EnumPopup(node.Affiliation, GUILayout.Width(editBoardNodeData_AffiliationWidth));
        var oldClr = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
		if (GUILayout.Button("X", GUILayout.Height(14f), GUILayout.Width(editBoardNodeData_DeleteButtonWidth)))
        {
            delete = true;
        }
        GUI.backgroundColor = oldClr;
        EditorGUILayout.EndHorizontal();
        return delete;
	}

	public static Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width*height];

		for(int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

}

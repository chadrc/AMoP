using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(EditorBoardNodeBehavior))]
public class EditorBoardNodeInspector : Editor
{
	private const float legendItemWidth = 80f;
	private const float legendItemHeight = 20f;
	private static bool showLegend;

	private EditorBoardNodeBehavior node;

    public override void OnInspectorGUI()
    {
        node = (EditorBoardNodeBehavior)target;

        AMoPEditorUtils.EditBoardNodeDataHeader();
        AMoPEditorUtils.EditBoardNodeData("Node: ", node.data);
    }

	private void OnSceneGUI()
	{
		drawLegend ();
	}

	private static void drawLegend()
	{
		var typeArray = (BoardNodeType[])Enum.GetValues (typeof(BoardNodeType));
		float legendHeight = typeArray.Length * legendItemHeight;

		Handles.BeginGUI ();
		if (showLegend)
		{
			if (GUILayout.Button ("Hide Legend", GUILayout.Width (legendItemWidth)))
			{
				showLegend = false;
				return;
			}

			var backClr = new Color (1f, 1f, 1f, .5f);
			var style = new GUIStyle ();
			style.normal.background = MakeTex ((int)legendItemWidth, (int)legendHeight, backClr);
			GUILayout.BeginVertical (style, GUILayout.Width (legendItemWidth), GUILayout.Height(legendHeight+20f));
			foreach (var type in typeArray)
			{
				Texture2D texture = new Texture2D(1, 1);
				texture.SetPixel(0,0,EditorBoardNodeBehavior.TypeColorMap [type]);
				texture.Apply();
				var oldBackground = GUI.skin.box.normal.background;
				GUI.skin.box.normal.background = texture;

				GUILayout.BeginHorizontal (GUILayout.Width (legendItemWidth));

				GUILayout.Label (type.ToString () + " -> ", GUILayout.Width(legendItemWidth - legendItemHeight));
				GUILayout.Box (GUIContent.none, GUILayout.Width(legendItemHeight));

				GUILayout.EndHorizontal ();

				GUI.skin.box.normal.background = oldBackground;
			}

			GUILayout.EndVertical ();
		}
		else
		{
			if (GUILayout.Button ("Show Legend", GUILayout.Width (legendItemWidth)))
			{
				showLegend = true;
			}
		}
		Handles.EndGUI ();
	}

	private static Texture2D MakeTex(int width, int height, Color col)
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

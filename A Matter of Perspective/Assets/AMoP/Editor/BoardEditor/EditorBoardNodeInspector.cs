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

	private const float controlsHeight = 150f;
	private const float controlsWidth = 300f;

	private const float boardRotateControlWidth = 100f;
	private const float boardRotateControlHeight = 100f;

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
		if (node == null || node.Data == null)
		{
			EditorGUILayout.LabelField ("No Data");
		}
		else
		{
			EditorGUI.BeginChangeCheck ();
			AMoPEditorUtils.EditBoardNodeDataHeader();
			bool delete = AMoPEditorUtils.EditBoardNodeData("Node: ", node.Data);

			if (EditorGUI.EndChangeCheck() || delete)
			{
				node.InspectorEdited (delete);
			}
		}
    }

	private void OnSceneGUI()
	{
		if (node == null || node.Data == null)
		{
			return;
		}

		Handles.BeginGUI ();
		drawLegend ();
		drawBoardRotator ();

		var sceneViewRect = EditorWindow.GetWindow<SceneView> ().camera.pixelRect;
		var controlRect = new Rect (0, sceneViewRect.height - controlsHeight, controlsWidth, controlsHeight);
		var backClr = new Color (1f, 1f, 1f, .5f);
		var style = new GUIStyle ();
		style.normal.background = MakeTex ((int)controlRect.width, (int)controlRect.height, backClr);
		GUILayout.BeginArea (controlRect, style);

		GUILayout.Label ("Controls [" + node.name + "]");

		GUILayout.BeginHorizontal (GUILayout.Width (controlsWidth));
		GUILayout.BeginVertical (GUILayout.Width(100f));

		Vector3 pos;
		pos.x = IncDecControl ("Pos X: ", node.Data.Position.x);
		pos.y = IncDecControl ("Pos Y: ", node.Data.Position.y);
		pos.z = IncDecControl ("Pos Z: ", node.Data.Position.z);
		node.Data.Position = pos;

		GUILayout.EndVertical ();

		GUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Energy:", GUILayout.Width(70f));
		node.Data.StartingEnergy = EditorGUILayout.IntField ((int)node.Data.StartingEnergy, GUILayout.Width (50f));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Type:", GUILayout.Width(70f));
		node.Data.Type = (BoardNodeType)EditorGUILayout.EnumPopup(node.Data.Type, GUILayout.Width(70f));
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label ("Affiliation:", GUILayout.Width(70f));
		node.Data.Affiliation = (BoardNodeAffiliation)EditorGUILayout.EnumPopup(node.Data.Affiliation, GUILayout.Width(70f));
		EditorGUILayout.EndHorizontal ();

		bool delete = false;
		var oldClr = GUI.backgroundColor;
		GUI.backgroundColor = Color.red;
		if (GUILayout.Button ("Delete Node", GUILayout.Width(100f)))
		{
			delete = true;
		}

		GUI.backgroundColor = oldClr;

		GUILayout.EndVertical ();

		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();

		if (GUI.changed || delete)
		{
			node.InspectorEdited (delete);
		}

		Handles.EndGUI ();
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

	private void drawBoardRotator()
	{
		var sceneViewRect = EditorWindow.GetWindow<SceneView> ().camera.pixelRect;
		var controlRect = new Rect (
			sceneViewRect.width - boardRotateControlWidth, 
			sceneViewRect.height - boardRotateControlHeight, 
			boardRotateControlWidth, 
			boardRotateControlHeight);
		
		var backClr = new Color (1f, 1f, 1f, .5f);
		var style = new GUIStyle ();
		style.normal.background = MakeTex ((int)controlRect.width, (int)controlRect.height, backClr);
		GUILayout.BeginArea (controlRect, style);

		EditorGUILayout.BeginVertical ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		EditorGUILayout.LabelField ("Rotate Board");
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Up", GUILayout.Width(40f)))
		{
			rotateBoardParent (Vector2.up);
		}
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Right", GUILayout.Width(40f)))
		{
			rotateBoardParent (Vector2.right);
		}

		if (GUILayout.Button ("Left", GUILayout.Width(40f)))
		{
			rotateBoardParent (Vector2.left);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Down", GUILayout.Width(40f)))
		{
			rotateBoardParent (Vector2.down);
		}
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace ();

		EditorGUILayout.EndVertical ();

		GUILayout.EndArea ();
	}

	private void drawLegend()
	{
		var typeArray = (BoardNodeType[])Enum.GetValues (typeof(BoardNodeType));
		float legendHeight = typeArray.Length * legendItemHeight;

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
	}

	private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width*height];

		for(int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

	private void rotateBoardParent(Vector2 dir)
	{
		var boardParent = GameObject.Find ("BoardParent");
		if (boardParent == null)
		{
			Debug.LogError ("No object named BoardParent to rotate.");
		}

		boardParent.transform.Rotate(Vector3.up, 90f * -dir.x, Space.World);
		boardParent.transform.Rotate(Vector3.right, 90f * dir.y, Space.World);
	}
}

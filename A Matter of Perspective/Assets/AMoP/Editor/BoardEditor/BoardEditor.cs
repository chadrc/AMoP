using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BoardEditor : EditorWindow
{
	private const float boardRotateControlWidth = 112f;
	private const float boardRotateControlHeight = 112f;

	private const float legendItemWidth = 80f;
	private const float legendItemHeight = 20f;
	private static bool showLegend;

    private BoardData boardData;
    private GameObject boardParent;
    private bool showNodeList;
    private List<BoardData> boardDatas = new List<BoardData>();
    private BoardEditorTabState tabState = BoardEditorTabState.Nodes;
    private Color highlightClr = new Color(.3f, .3f, .3f);
    private GUIStyle whiteText;
    private Vector2 boardScrollPos;
    private Vector2 nodeScrollPos;
	private List<EditorBoardNodeBehavior> editNodes;
	private bool sceneOrtho;

    private enum BoardEditorTabState
    {
        Nodes,
        Stats,
        Actions
    }
    
    [MenuItem("AMoP/Board Editor")]
    static void Init()
    {
        var window = (BoardEditor)EditorWindow.GetWindow(typeof(BoardEditor));
		window.name = "Board Editor";
        window.Show();
    }

    void OnFocus()
	{
		SceneView.onSceneGUIDelegate += onSceneGUI;
		loadBoardDatas();
		setupListeners ();

		checkSceneCam ();
    }

    void OnDestroy()
    {
		unloadBoard ();
    }

    void OnGUI()
    {
		setupListeners ();

        // Can only call GUI functions from inside OnGUI
        whiteText = new GUIStyle(GUI.skin.button);
        whiteText.normal.textColor = new Color(.9f, .9f, .9f);

        if (boardData == null)
        {
            if (GUILayout.Button("Create"))
            {
                createBoard();
            }

            if (boardDatas == null)
            {
                loadBoardDatas();
            }

            boardScrollPos = EditorGUILayout.BeginScrollView(boardScrollPos);
            foreach(var data in boardDatas)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(data.name);
                if (GUILayout.Button("Load")) {
                    loadBoard(data);
                }

                var oldClr = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(20f)))
                {
                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data));
                    loadBoardDatas();
                    return;
                }
                GUI.backgroundColor = oldClr;
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
        else
        {
			checkSceneCam ();

            if (GUILayout.Button("Unload Board"))
            {
                unloadBoard();
                return;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.PrefixLabel(boardData.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            boardData.BoardSize = EditorGUILayout.IntSlider("Board Size: ", boardData.BoardSize, 3, 6);

            var originalClr = GUI.backgroundColor;
            EditorGUILayout.BeginHorizontal();

            foreach(var e in (BoardEditorTabState[])Enum.GetValues(typeof(BoardEditorTabState)))
            {
                GUI.backgroundColor = e == tabState ? highlightClr : originalClr;
                bool pressed = e == tabState ? GUILayout.Button(e.ToString(), whiteText) : GUILayout.Button(e.ToString());
                if (pressed)
                {
                    tabState = e;
                }
                GUI.backgroundColor = originalClr;
            }

            EditorGUILayout.EndHorizontal();
            
            switch (tabState)
            {
                case BoardEditorTabState.Nodes:
                    NodeTabState();
                    break;

                case BoardEditorTabState.Stats:
                    StatsTabState();
                    break;

                case BoardEditorTabState.Actions:
                    ActionsTabState();
                    break;
			}
        }
    }

    private void NodeTabState()
    {
        if (GUILayout.Button("Add Node"))
        {
            addNewNode();
        }

        AMoPEditorUtils.EditBoardNodeDataHeader();

        BoardNodeData deleteNode = null;

        int index = 0;
        nodeScrollPos = EditorGUILayout.BeginScrollView(nodeScrollPos);
        foreach (var node in boardData.Nodes)
        {
            if (AMoPEditorUtils.EditBoardNodeData(index.ToString("000") + ": ", node, boardData.BoardSize))
            {
                deleteNode = node;
            }

			bool deleted = deleteNode == node;
			if (GUI.changed && index < editNodes.Count)
			{
				editNodes [index].InspectorEdited (deleted);
			}

            index++;
        }
        EditorGUILayout.EndScrollView();

        if (deleteNode != null)
        {
            boardData.RemoveNode(deleteNode);
            reloadScene();
        }
    }

    void OnProjectChanged()
    {
        loadBoardDatas();
    }

    private void StatsTabState()
    {
        EditorGUILayout.LabelField("Scores");

        foreach (var pair in boardData.Scores)
        {
            EditorGUILayout.IntField(pair.Key.ToString(), pair.Value);
        }
    }

    private void ActionsTabState()
    {

    }

	private void onSceneGUI(SceneView scene)
	{
		setupListeners ();
		checkSceneCam ();

		if (boardData != null)
		{
			drawLegend ();
			drawBoardRotator ();
		}
	}

	private void checkSceneCam()
	{
		var allCams = SceneView.GetAllSceneCameras ();
		if (allCams.Length > 0)
		{
			if (sceneOrtho != allCams [0].orthographic)
			{
				sceneOrtho = allCams [0].orthographic;
				hideShowNodes ();
			}
		}
	}

	private void setupListeners()
	{
		if (EditorBoardNodeBehavior.GetBoardNodeData == null)
		{
			EditorBoardNodeBehavior.GetBoardNodeData += editNodeGetDataDelegate;
			EditorBoardNodeBehavior.Edited += onNodeEdited;
		}
	}

    private void loadBoardDatas()
    {
        boardDatas.Clear();

        var boardAssets = AssetDatabase.FindAssets("t:BoardData");

        foreach (var boardGUID in boardAssets)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(boardGUID);
            var asset = AssetDatabase.LoadAssetAtPath<BoardData>(assetPath);
            boardDatas.Add(asset);
        }
    }

    private void createBoard()
    {
        boardData = AMoPMenuItem.CreateBoardData();
        loadBoardDatas();
    }

    private void loadBoard(BoardData data)
    {
        boardData = data;
        createSceneBoard();
    }

    private void unloadBoard()
    {
        boardData = null;
        destorySceneBoard();
    }

    private void reloadScene()
    {
        destorySceneBoard();
        createSceneBoard();
    }

    private void createSceneBoard()
    {
		var boardParent = GameObject.Find("BoardParent");
		boardParent.transform.rotation = Quaternion.identity;
        if (boardParent == null)
        {
            Debug.Log("Creating board parent.");
            boardParent = new GameObject("BoardParent");
        }

		editNodes = new List<EditorBoardNodeBehavior> ();
        var nodes = boardData.Nodes;
        // Create edit nodes
		for (int i=0; i<nodes.Count; i++)
        {
            var obj = new GameObject();
            obj.transform.SetParent(boardParent.transform);
            var editNode = obj.AddComponent<EditorBoardNodeBehavior>();
			editNodes.Add (editNode);
			editNode.SetData (i, boardData);
        }

		hideShowNodes ();
    }

	private void showAllNodes()
	{
		for (int x = 0; x < boardData.BoardSize; x++)
		{
			for (int y = 0; y < boardData.BoardSize; y++)
			{
                var row = AMoPUtils.GetEditNodeRow(editNodes, x - boardData.OffsetValue, y - boardData.OffsetValue);
				row.Closest.Show ();
				foreach (var h in row.Hidden)
				{
					h.Show ();
				}
			}
		}
	}

	private void hideShowNodes()
	{
		if (editNodes == null)
		{
			return;
		}

		for (int x = 0; x < boardData.BoardSize; x++)
		{
			for (int y = 0; y < boardData.BoardSize; y++)
			{
                var row = AMoPUtils.GetEditNodeRow(editNodes, x - boardData.OffsetValue, y - boardData.OffsetValue);
				if (row.Closest != null)
				{
					row.Closest.Show ();
				}
				foreach (var h in row.Hidden)
				{
					if (sceneOrtho)
					{
						h.Hide ();
					}
					else
					{
						h.Fade ();
					}
				}
			}
		}
	}

    private void destorySceneBoard()
    {
        var boardParent = GameObject.Find("BoardParent");
        var destoryList = new List<GameObject>();

        // Hack for destorying all child objects in edit mode
        // DestroyImmediate in this loop ends up skipping every other node
        foreach (Transform t in boardParent.transform)
        {
            destoryList.Add(t.gameObject);
        }

        foreach (var g in destoryList)
        {
            GameObject.DestroyImmediate(g);
        }

		boardParent.transform.rotation = Quaternion.identity;
    }

    private void addNewNode()
    {
        boardData.AddNode();
        reloadScene();
    }

	private BoardNodeData editNodeGetDataDelegate(int index)
	{
		if (boardData != null && index >= 0 && index < boardData.Nodes.Count)
		{
			return boardData.Nodes [index];
		}
		return null;
	}

	private void onNodeEdited(EditorBoardNodeBehavior editNode, bool delete)
	{
		if (delete)
		{
			boardData.RemoveNode (editNode.NodeIndex);
			editNodes.Remove (editNode);
			GameObject.DestroyImmediate (editNode.gameObject);

			int index = 0;
			for (int i=0; i<editNodes.Count; i++)
			{
				editNodes [index].SetData (index, boardData);
			}
		}

		hideShowNodes ();
		EditorUtility.SetDirty (this);
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
		style.normal.background = AMoPEditorUtils.MakeTex ((int)controlRect.width, (int)controlRect.height, backClr);
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
		if (GUILayout.Button ("Up", GUILayout.Width(50f)))
		{
			rotateBoardParent (Vector2.up);
		}
		GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Right", GUILayout.Width(50f)))
		{
			rotateBoardParent (Vector2.right);
		}

		if (GUILayout.Button ("Left", GUILayout.Width(50f)))
		{
			rotateBoardParent (Vector2.left);
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Down", GUILayout.Width(50f)))
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

		GUILayout.BeginArea (new Rect (0, 0, legendItemWidth+20f, legendHeight+70f));

		if (showLegend)
		{
			if (GUILayout.Button ("Hide Legend", GUILayout.Width (legendItemWidth)))
			{
				showLegend = false;
				return;
			}

			var backClr = new Color (1f, 1f, 1f, .5f);
			var style = new GUIStyle ();
			style.normal.background = AMoPEditorUtils.MakeTex ((int)legendItemWidth, (int)legendHeight, backClr);
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

		GUILayout.EndArea ();
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

		hideShowNodes ();
	}
}

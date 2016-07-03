using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BoardEditor : EditorWindow
{
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

    private enum BoardEditorTabState
    {
        Nodes,
        Stats,
        Actions
    }

    // Add menu named "My Window" to the Window menu
    [MenuItem("AMoP/Board Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = (BoardEditor)EditorWindow.GetWindow(typeof(BoardEditor));
		window.name = "Board Editor";
		SceneView.onSceneGUIDelegate += window.onSceneGUI;
        window.Show();
    }

    void OnFocus()
    {
		loadBoardDatas();
		setupListeners ();
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
            if (AMoPEditorUtils.EditBoardNodeData(index.ToString("000") + ": ", node))
            {
                deleteNode = node;
            }

			bool deleted = deleteNode == node;
			if (GUI.changed)
			{
				editNodes [index].InspectorEdited (deleted);
			}

            index++;
        }
        EditorGUILayout.EndScrollView();

        if (deleteNode != null)
        {
            boardData.RemoveNode(deleteNode);
        }
    }

    void OnProjectChanged()
    {
        loadBoardDatas();
    }

    private void StatsTabState()
    {

    }

    private void ActionsTabState()
    {

    }

	private void onSceneGUI(SceneView scene)
	{
		setupListeners ();
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
        int i = 0;
        foreach (var node in nodes)
        {
            var obj = new GameObject();
            obj.transform.SetParent(boardParent.transform);
            var editNode = obj.AddComponent<EditorBoardNodeBehavior>();
			editNodes.Add (editNode);
			editNode.SetData (i);
            i++;
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
			foreach (var data in boardData.Nodes)
			{
				editNodes [index].SetData (index);
				index++;
			}
		}
		EditorUtility.SetDirty (this);
	}
}

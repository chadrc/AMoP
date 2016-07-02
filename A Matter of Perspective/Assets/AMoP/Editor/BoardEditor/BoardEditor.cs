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
        window.Show();
    }

    void OnFocus()
    {
        loadBoardDatas();
    }

    void OnDestroy()
    {
		unloadBoard ();
    }

    void OnGUI()
    {
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
        createBoard();
    }

    private void createSceneBoard()
    {
        var boardParent = GameObject.Find("BoardParent");
        if (boardParent == null)
        {
            Debug.Log("Creating board parent.");
            boardParent = new GameObject("BoardParent");
        }

        var nodes = boardData.Nodes;
        // Create edit nodes
        int i = 0;
        foreach (var node in nodes)
        {
            var obj = new GameObject();
            obj.name = "Board Node: " + i.ToString("000");
            obj.transform.SetParent(boardParent.transform);
            var editNode = obj.AddComponent<EditorBoardNodeBehavior>();
            editNode.data = node;
            editNode.transform.position = node.Position;
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
    }

    private void addNewNode()
    {
        boardData.AddNode();
    }
}

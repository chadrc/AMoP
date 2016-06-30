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
        window.Show();
    }

    void OnEnable()
    {
        loadBoardDatas();
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

            foreach(var data in boardDatas)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(data.name);
                if (GUILayout.Button("Load")) {
                    boardData = data;
                }

                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            if (GUILayout.Button("Unload"))
            {
                boardData = null;
                return;
            }

            boardData.BoardName = EditorGUILayout.TextField("Board Name: ", boardData.BoardName);
            EditorGUILayout.LabelField("Description:");
            boardData.Description = EditorGUILayout.TextArea(boardData.Description, GUILayout.Height(50f));

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

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Index", GUILayout.Width(40f));
        EditorGUILayout.LabelField("Position", GUILayout.Width(130f));
        EditorGUILayout.LabelField("Energy", GUILayout.Width(60f));
        EditorGUILayout.LabelField("Type", GUILayout.Width(70f));
        EditorGUILayout.LabelField("Affiliation", GUILayout.Width(70f));

        EditorGUILayout.EndHorizontal();

        BoardNodeData deleteNode = null;

        int index = 0;
        foreach (var node in boardData.Nodes)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(index.ToString("000") + ": ", GUILayout.Width(40f));
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
                deleteNode = node;
            }
            GUI.backgroundColor = oldClr;
            EditorGUILayout.EndHorizontal();
            index++;
        }

        if (deleteNode != null)
        {
            boardData.RemoveNode(deleteNode);
        }
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

        var boardParent = GameObject.Find("BoardParent");
        if (boardParent == null)
        {
            Debug.Log("Creating board parent.");
            boardParent = new GameObject("BoardParent");
        }

        // Create edit nodes
    }

    private void addNewNode()
    {
        boardData.AddNode();
    }
}

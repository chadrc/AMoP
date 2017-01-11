using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class BoardEditor : EditorWindow
{
	private const float BoardRotateControlWidth = 112f;
	private const float BoardRotateControlHeight = 112f;

	private const float LegendItemWidth = 80f;
	private const float LegendItemHeight = 20f;
	private static bool _showLegend;

    private BoardData _boardData;
    private bool _showSeriesList;
    private Dictionary<BoardSeries, bool> _showSeriesBools = new Dictionary<BoardSeries, bool>();
    private readonly List<BoardData> _boardDatas = new List<BoardData>();
    private List<BoardSeries> _boardSeries = new List<BoardSeries>();
    private BoardEditorTabState _tabState = BoardEditorTabState.Nodes;
    private readonly Color _highlightClr = new Color(.3f, .3f, .3f);
    private GUIStyle _whiteText;
    private Vector2 _boardScrollPos;
    private Vector2 _nodeScrollPos;
	private List<EditorBoardNodeBehavior> _editNodes;
	private bool _sceneOrtho;

    private enum BoardEditorTabState
    {
        Nodes,
        Stats,
        Actions
    }
    
    [MenuItem("AMoP/Board Editor")]
    private static void Init()
    {
        var window = (BoardEditor)EditorWindow.GetWindow(typeof(BoardEditor));
		window.name = "Board Editor";
        window.Show();
    }

    private void OnFocus()
	{
        SceneView.onSceneGUIDelegate += OnSceneGui;
        LoadBoardDatas();
        SetupListeners();

        //checkSceneCam();
    }

    private void OnDestroy()
    {
		UnloadBoard ();
    }

    private void OnGUI()
    {
		SetupListeners ();

        // Can only call GUI functions from inside OnGUI
        _whiteText = new GUIStyle(GUI.skin.button) {normal = {textColor = new Color(.9f, .9f, .9f)}};

        if (_boardData == null)
        {
            if (GUILayout.Button("Create"))
            {
                CreateBoard();
            }

            if (_boardDatas == null)
            {
                LoadBoardDatas();
            }

            EditorGUILayout.BeginHorizontal();
            var originalClr = GUI.backgroundColor;

            GUI.backgroundColor = !_showSeriesList ? _highlightClr : originalClr;
            var pressed = !_showSeriesList ? GUILayout.Button("All Boards", _whiteText) : GUILayout.Button("All Boards");
            if (pressed)
            {
                _showSeriesList = false;
            }
            GUI.backgroundColor = originalClr;

            GUI.backgroundColor = _showSeriesList ? _highlightClr : originalClr;
            pressed = _showSeriesList ? GUILayout.Button("Series", _whiteText) : GUILayout.Button("Series");
            if (pressed)
            {
                _showSeriesList = true;
            }
            GUI.backgroundColor = originalClr;
            EditorGUILayout.EndHorizontal();

            if (_showSeriesList)
            {
                _boardScrollPos = EditorGUILayout.BeginScrollView(_boardScrollPos);
                foreach (var series in _boardSeries)
                {
                    if (!_showSeriesBools.ContainsKey(series))
                    {
                        _showSeriesBools.Add(series, false);
                    }

                    _showSeriesBools[series] = EditorGUILayout.Foldout(_showSeriesBools[series], series.DisplayName);
                    if (!_showSeriesBools[series]) continue;
                    foreach(var board in series)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(board.name);
                        if (GUILayout.Button("Load")) {
                            LoadBoard(board);
                        }

                        var oldClr = GUI.backgroundColor;
                        GUI.backgroundColor = Color.red;
                        if (GUILayout.Button("X", GUILayout.Width(20f)))
                        {
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(board));
                            LoadBoardDatas();
                            return;
                        }
                        GUI.backgroundColor = oldClr;
                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                _boardScrollPos = EditorGUILayout.BeginScrollView(_boardScrollPos);
                foreach(var data in _boardDatas)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(data.name);
                    if (GUILayout.Button("Load")) {
                        LoadBoard(data);
                    }

                    var oldClr = GUI.backgroundColor;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("X", GUILayout.Width(20f)))
                    {
                        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(data));
                        LoadBoardDatas();
                        return;
                    }
                    GUI.backgroundColor = oldClr;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
        }
        else
        {
            CheckSceneCam();

            if (GUILayout.Button("Unload Board"))
            {
                UnloadBoard();
                return;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.PrefixLabel(_boardData.name);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            _boardData.BoardSize = EditorGUILayout.IntSlider("Board Size: ", _boardData.BoardSize, 3, 6);

            var originalClr = GUI.backgroundColor;
            EditorGUILayout.BeginHorizontal();

            foreach (var e in (BoardEditorTabState[])Enum.GetValues(typeof(BoardEditorTabState)))
            {
                GUI.backgroundColor = e == _tabState ? _highlightClr : originalClr;
                var pressed = e == _tabState ? GUILayout.Button(e.ToString(), _whiteText) : GUILayout.Button(e.ToString());
                if (pressed)
                {
                    _tabState = e;
                }
                GUI.backgroundColor = originalClr;
            }

            EditorGUILayout.EndHorizontal();

            switch (_tabState)
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

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void NodeTabState()
    {
        if (GUILayout.Button("Add Node"))
        {
            AddNewNode();
        }

        AMoPEditorUtils.EditBoardNodeDataHeader();

        BoardNodeData deleteNode = null;

        var index = 0;
        _nodeScrollPos = EditorGUILayout.BeginScrollView(_nodeScrollPos);
        foreach (var node in _boardData.Nodes)
        {
            if (AMoPEditorUtils.EditBoardNodeData(index.ToString("000") + ": ", node, _boardData.BoardSize))
            {
                deleteNode = node;
            }

			var deleted = deleteNode == node;
            if (GUI.changed && index < _editNodes.Count)
            {
                _editNodes[index].InspectorEdited(deleted);
            }

            index++;
        }
        EditorGUILayout.EndScrollView();

        if (deleteNode == null) return;
        Debug.Log("Deleting Node");
        _boardData.RemoveNode(deleteNode);
        ReloadScene();
    }

    private void StatsTabState()
    {
        _boardData.InfoClassName = EditorGUILayout.TextField("Info Class Name:", _boardData.InfoClassName);

        EditorGUILayout.LabelField("Scores");

        foreach (var pair in _boardData.Scores)
        {
            var score = EditorGUILayout.IntField(pair.Key.ToString(), pair.Value);
            _boardData.Scores.SetScore(pair.Key, score);
        }
    }

    private void ActionsTabState()
    {

    }

    private void OnProjectChanged()
    {
        LoadBoardDatas();
    }

    private void OnSceneGui(SceneView scene)
	{
        SetupListeners();
        CheckSceneCam();

	    if (_boardData == null) return;
	    DrawLegend();
	    DrawBoardRotator();
	}

	private void CheckSceneCam()
	{
		var allCams = SceneView.GetAllSceneCameras ();
	    if (allCams.Length <= 0) return;
	    if (_sceneOrtho == allCams[0].orthographic) return;
	    _sceneOrtho = allCams [0].orthographic;
	    HideShowNodes ();
	}

	private void SetupListeners()
	{
	    EditorBoardNodeBehavior.GetBoardNodeData = null;
        EditorBoardNodeBehavior.Edited -= OnNodeEdited;

        EditorBoardNodeBehavior.GetBoardNodeData += EditNodeGetDataDelegate;
		EditorBoardNodeBehavior.Edited += OnNodeEdited;
	}

    private void LoadBoardDatas()
    {
        _boardDatas.Clear();
        _boardSeries.Clear();

        var boardAssets = AssetDatabase.FindAssets("t:BoardData");
        var boardSeries = AssetDatabase.FindAssets("t:BoardSeries");

        foreach (var guid in boardAssets)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<BoardData>(assetPath);
            _boardDatas.Add(asset);
        }

        foreach (var guid in boardSeries)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<BoardSeries>(assetPath);
            _boardSeries.Add(asset);
        }
    }

    private void CreateBoard()
    {
        _boardData = AMoPMenuItem.CreateBoardData();
        LoadBoardDatas();
    }

    private void LoadBoard(BoardData data)
    {
        _boardData = data;
        CreateSceneBoard();
    }

    private void UnloadBoard()
    {
        _boardData = null;
        destorySceneBoard();
    }

    private void ReloadScene()
    {
        destorySceneBoard();
        CreateSceneBoard();
    }

    private void CreateSceneBoard()
    {
		var boardParent = GameObject.Find("BoardParent");
		boardParent.transform.rotation = Quaternion.identity;

        _editNodes = new List<EditorBoardNodeBehavior> ();
        var nodes = _boardData.Nodes;
        // Create edit nodes
        for (int i=0; i<nodes.Count; i++)
        {
            var obj = new GameObject();
            obj.transform.SetParent(boardParent.transform);
            var editNode = obj.AddComponent<EditorBoardNodeBehavior>();
			_editNodes.Add (editNode);
			editNode.SetData (i, _boardData);
        }

		HideShowNodes ();

        SetupListeners();
    }

	private void ShowAllNodes()
	{
		for (var x = 0; x < _boardData.BoardSize; x++)
		{
			for (var y = 0; y < _boardData.BoardSize; y++)
			{
                var row = AMoPUtils.GetEditNodeRow(_editNodes, x - _boardData.OffsetValue, y - _boardData.OffsetValue);
				row.Closest.Show ();
				foreach (var h in row.Hidden)
				{
					h.Show ();
				}
			}
		}
	}

	private void HideShowNodes()
	{
		if (_editNodes == null)
		{
			return;
		}

		for (var x = 0; x < _boardData.BoardSize; x++)
		{
			for (var y = 0; y < _boardData.BoardSize; y++)
			{
                var row = AMoPUtils.GetEditNodeRow(_editNodes, x - _boardData.OffsetValue, y - _boardData.OffsetValue);
				if (row.Closest != null)
				{
					row.Closest.Show ();
				}
				foreach (var h in row.Hidden)
				{
					if (_sceneOrtho)
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

    private static void destorySceneBoard()
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

    private void AddNewNode()
    {
        _boardData.AddNode();
        ReloadScene();
    }

	private BoardNodeData EditNodeGetDataDelegate(int index)
	{
		if (_boardData != null && index >= 0 && index < _boardData.Nodes.Count)
		{
			return _boardData.Nodes [index];
		}
		return null;
	}

	private void OnNodeEdited(EditorBoardNodeBehavior editNode, bool delete)
	{
		if (delete)
		{
			_boardData.RemoveNode (editNode.NodeIndex);
			_editNodes.Remove (editNode);
			GameObject.DestroyImmediate (editNode.gameObject);

			int index = 0;
			for (int i=0; i<_editNodes.Count; i++)
			{
				_editNodes [index].SetData (index, _boardData);
			}
		}

		HideShowNodes ();
		EditorUtility.SetDirty (this);
	}


	private void DrawBoardRotator()
	{
        //var sceneViewRect = EditorWindow.GetWindow<SceneView>().camera.pixelRect;
        var sceneViewRect = Camera.current.pixelRect;
        var controlRect = new Rect(
            sceneViewRect.width - BoardRotateControlWidth,
            sceneViewRect.height - BoardRotateControlHeight,
            BoardRotateControlWidth,
            BoardRotateControlHeight);

        var backClr = new Color(1f, 1f, 1f, .5f);
        var style = new GUIStyle();
        style.normal.background = AMoPEditorUtils.MakeTex((int)controlRect.width, (int)controlRect.height, backClr);
        GUILayout.BeginArea(controlRect, style);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Rotate Board");
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Up", GUILayout.Width(50f)))
        {
            RotateBoardParent(Vector2.up);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Right", GUILayout.Width(50f)))
        {
            RotateBoardParent(Vector2.right);
        }

        if (GUILayout.Button("Left", GUILayout.Width(50f)))
        {
            RotateBoardParent(Vector2.left);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Down", GUILayout.Width(50f)))
        {
            RotateBoardParent(Vector2.down);
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();

        EditorGUILayout.EndVertical();

        GUILayout.EndArea();
    }

	private static void DrawLegend()
	{
        var typeArray = (BoardNodeType[])Enum.GetValues(typeof(BoardNodeType));
        var legendHeight = typeArray.Length * LegendItemHeight;

        GUILayout.BeginArea(new Rect(0, 0, LegendItemWidth + 20f, legendHeight + 70f));

        if (_showLegend)
        {
            if (GUILayout.Button("Hide Legend", GUILayout.Width(LegendItemWidth)))
            {
                _showLegend = false;
                return;
            }

            var backClr = new Color(1f, 1f, 1f, .5f);
            var style = new GUIStyle
            {
                normal = {background = AMoPEditorUtils.MakeTex((int) LegendItemWidth, (int) legendHeight, backClr)}
            };
            GUILayout.BeginVertical(style, GUILayout.Width(LegendItemWidth), GUILayout.Height(legendHeight + 20f));
            foreach (var type in typeArray)
            {
                var texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, EditorBoardNodeBehavior.TypeColorMap[type]);
                texture.Apply();
                var oldBackground = GUI.skin.box.normal.background;
                GUI.skin.box.normal.background = texture;

                GUILayout.BeginHorizontal(GUILayout.Width(LegendItemWidth));

                GUILayout.Label(type.ToString() + " -> ", GUILayout.Width(LegendItemWidth - LegendItemHeight));
                GUILayout.Box(GUIContent.none, GUILayout.Width(LegendItemHeight));

                GUILayout.EndHorizontal();

                GUI.skin.box.normal.background = oldBackground;
            }

            GUILayout.EndVertical();
        }
        else
        {
            if (GUILayout.Button("Show Legend", GUILayout.Width(LegendItemWidth)))
            {
                _showLegend = true;
            }
        }

        GUILayout.EndArea();
    }

    private void RotateBoardParent(Vector2 dir)
	{
		var boardParent = GameObject.Find ("BoardParent");
		if (boardParent == null)
		{
			Debug.LogError ("No object named BoardParent to rotate.");
		}

	    if (boardParent != null)
	    {
	        boardParent.transform.Rotate(Vector3.up, 90f * -dir.x, Space.World);
	        boardParent.transform.Rotate(Vector3.right, 90f * dir.y, Space.World);
	    }

	    HideShowNodes ();
	}
}

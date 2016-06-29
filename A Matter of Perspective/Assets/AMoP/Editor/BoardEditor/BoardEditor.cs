using UnityEngine;
using UnityEditor;
using System.Collections;

public class BoardEditor : EditorWindow
{
    private BoardData boardData;
    private GameObject boardParent;
    private bool showNodeList;

    // Add menu named "My Window" to the Window menu
    [MenuItem("AMoP/Board Editor")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        var window = (BoardEditor)EditorWindow.GetWindow(typeof(BoardEditor));
        window.Show();
    }

    void OnGUI()
    {
        if (boardData == null)
        {
            if (GUILayout.Button("Create"))
            {
                createBoard();
            }

            if (GUILayout.Button("Load"))
            {

            }
        }
        else
        {
            EditorGUILayout.TextField("Board Name: ", boardData.BoardName);
            EditorGUILayout.TextField("Description: ", boardData.Description);
            if (GUILayout.Button("Add Node"))
            {
                addNewNode();
            }

            showNodeList = EditorGUILayout.Foldout(showNodeList, "Node List");    
            if (showNodeList)
            {
                int index = 0;
                foreach(var node in boardData.Nodes)
                {
                    EditorGUILayout.BeginHorizontal(GUILayout.Width(400f));

                    EditorGUILayout.PrefixLabel(index.ToString("000") + ": ");
                    string[] posOptions = { "0", "1", "2", "3", "4", "5" };
                    int[] posOptionsVals = { 0, 1, 2, 3, 4, 5 };
                    int x = EditorGUILayout.IntPopup((int)node.Position.x, posOptions, posOptionsVals);
                    int y = EditorGUILayout.IntPopup((int)node.Position.y, posOptions, posOptionsVals);
                    int z = EditorGUILayout.IntPopup((int)node.Position.z, posOptions, posOptionsVals);
                    node.Position = new Vector3(x, y, z);
                    node.StartingEnergy = EditorGUILayout.FloatField(node.StartingEnergy);
                    node.Type = (BoardNodeType)EditorGUILayout.EnumPopup(node.Type);
                    node.Affiliation = (BoardNodeAffiliation)EditorGUILayout.EnumPopup(node.Affiliation);
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    index++;
                }
            }
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

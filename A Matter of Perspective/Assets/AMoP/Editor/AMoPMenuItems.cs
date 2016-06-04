using UnityEngine;
using UnityEditor;

public class CreateBoardDataMenuItem : MonoBehaviour {
    
    [MenuItem("AMoP/Create Board Data")]
    public static void CreateBoardData()
    {
        BoardData data = ScriptableObject.CreateInstance<BoardData>();
        AssetDatabase.CreateAsset(data, AssetDatabase.GenerateUniqueAssetPath("Assets/AMoP/Data/Boards/BoardData.asset"));
        Selection.activeObject = data;
    }

    [MenuItem("AMoP/Create Board Node Factory")]
    public static void CreateBoardNodeFactory()
    {
        BoardNodeFactory data = ScriptableObject.CreateInstance<BoardNodeFactory>();
        AssetDatabase.CreateAsset(data, "Assets/AMoP/Data/BoardNodeFactory.asset");
        Selection.activeObject = data;
    }
}

using UnityEngine;
using UnityEditor;

public class AMoPMenuItem: MonoBehaviour {
    public static string BaseDataPath = "Assets/AMoP/Data/";
    
    [MenuItem("AMoP/Create Board Data")]
    public static void CreateBoardData()
    {
        CreateUniqueSO<BoardData>("Boards/BoardData.asset");
    }

    [MenuItem("AMoP/Create Board Node Factory")]
    public static void CreateBoardNodeFactory()
    {
        CreateUniqueSO<BoardNodeFactory>("Board Node Factories/BoardNodeFactory.asset");
    }

    [MenuItem("AMoP/Create Energy Factory")]
    public static void CreateEnergyFactory()
    {
        CreateUniqueSO<EnergyFactory>("Energy Factories/EnergyFactory.asset");
    }

    private static void CreateUniqueSO<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(data, AssetDatabase.GenerateUniqueAssetPath(BaseDataPath + path));
        Selection.activeObject = data;
    }
}

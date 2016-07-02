using UnityEngine;
using UnityEditor;

public class AMoPMenuItem: MonoBehaviour {
    public static string BaseDataPath = "Assets/AMoP/Data/";
    
    [MenuItem("AMoP/Create Board Data")]
    public static void CreateBoardData()
    {
        CreateDataSO<BoardData>("Boards/BoardData.asset");
    }

    [MenuItem("AMoP/Create Board Node Factory")]
    public static void CreateBoardNodeFactory()
    {
        CreateDataSO<BoardNodeFactory>("Board Node Factories/BoardNodeFactory.asset");
    }

    [MenuItem("AMoP/Create Energy Factory")]
    public static void CreateEnergyFactory()
    {
        CreateDataSO<EnergyFactory>("Energy Factories/EnergyFactory.asset");
    }

    [MenuItem("AMoP/Create Board Series")]
    public static void CreateBoardSeries()
    {
        CreateDataSO<BoardSeries>("Board Series/Series.asset");
    }

    [MenuItem("AMoP/Create Board Series List")]
    public static void CreateBoardSeriesList()
    {
        try
        {
            CreateUniqueDataSO<BoardSeriesList>("BoardSeriesList.asset");
        } catch (System.Exception)
        {
            Debug.LogError("Cannot create more than one Board Series List.");
        }
    }

    private static void CreateDataSO<T>(string path) where T : ScriptableObject
    {
        CreateSO<T>(AssetDatabase.GenerateUniqueAssetPath(BaseDataPath + path));
    }

    private static void CreateUniqueDataSO<T>(string path) where T : ScriptableObject
    {
        CreateSO<T>(BaseDataPath + path);
    }

    private static void CreateSO<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(data, path);
        Selection.activeObject = data;
    }
}

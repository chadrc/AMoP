using UnityEngine;
using UnityEditor;

public class AMoPMenuItem: MonoBehaviour {
    public static string BaseDataPath = "Assets/AMoP/Data/";
    
    [MenuItem("AMoP/Create Board Data")]
    public static BoardData CreateBoardData()
    {
        return CreateDataSO<BoardData>("Boards/BoardData.asset");
    }

    [MenuItem("AMoP/Create Board Node Factory")]
    public static BoardNodeFactory CreateBoardNodeFactory()
    {
        return CreateDataSO<BoardNodeFactory>("Board Node Factories/BoardNodeFactory.asset");
    }

    [MenuItem("AMoP/Create Energy Factory")]
    public static EnergyFactory CreateEnergyFactory()
    {
        return CreateDataSO<EnergyFactory>("Energy Factories/EnergyFactory.asset");
    }

    [MenuItem("AMoP/Create Board Series")]
    public static BoardSeries CreateBoardSeries()
    {
        return CreateDataSO<BoardSeries>("Board Series/Series.asset");
    }

    [MenuItem("AMoP/Create Board Series List")]
    public static BoardSeriesList CreateBoardSeriesList()
    {
        BoardSeriesList list = null;
        try
        {
            list = CreateUniqueDataSO<BoardSeriesList>("BoardSeriesList.asset");
        } catch (System.Exception)
        {
            Debug.LogError("Cannot create more than one Board Series List.");
        }
        return list;
    }

    [MenuItem("AMoP/Create Game Constants")]
    public static GameConstants CreateGameConstants()
    {
        return CreateDataSO<GameConstants>("GameConstants.asset");
    }

    private static T CreateDataSO<T>(string path) where T : ScriptableObject
    {
        return CreateSO<T>(AssetDatabase.GenerateUniqueAssetPath(BaseDataPath + path));
    }

    private static T CreateUniqueDataSO<T>(string path) where T : ScriptableObject
    {
        return CreateSO<T>(BaseDataPath + path);
    }

    public static T CreateSO<T>(string path) where T : ScriptableObject
    {
        T data = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(data, path);
        Selection.activeObject = data;
        return data;
    }
}

using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public static BoardSeriesList SeriesList { get { return Instance.seriesList; } }
    public static GameConstants Constants { get { return Instance.constants; } }

    [SerializeField]
    private BoardSeriesList seriesList;

    [SerializeField]
    private GameConstants constants;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.InvalidOperationException("Cannot create more than one GameData object.");
        }
    }
}

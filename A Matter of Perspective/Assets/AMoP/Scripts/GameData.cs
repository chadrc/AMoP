using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public static BoardSeriesList SeriesList { get { return Instance.seriesList; } }

    [SerializeField]
    private BoardSeriesList seriesList;

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

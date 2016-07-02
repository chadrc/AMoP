using UnityEngine;
using System.Collections.Generic;

public class BoardSeriesList : ScriptableObject
{
    [SerializeField]
    List<BoardSeries> seriesList;

    public int Count { get { return seriesList.Count; } }

    public BoardSeries GetSeries(int index)
    {
        return index < Count && index >= 0 ? seriesList[index] : null;
    }
}
 
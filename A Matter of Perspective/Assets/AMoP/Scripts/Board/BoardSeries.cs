using UnityEngine;
using System.Collections.Generic;

public class BoardSeries : ScriptableObject
{
    [SerializeField]
    string displayName;

    [SerializeField]
    List<BoardData> boards;

    public string DisplayName { get { return displayName; } }

    public int Count { get { return boards.Count; } }

    public BoardData GetBoard(int index)
    {
        return index < Count && index >=0 ? boards[index] : null;
    }
}

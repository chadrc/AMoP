using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BoardSeries : ScriptableObject, IEnumerable<BoardData>
{
    [SerializeField] private string _displayName;

    [SerializeField] private List<BoardData> _boards;

    public string DisplayName { get { return _displayName; } }

    public int Count { get { return _boards.Count; } }

    public BoardData GetBoard(int index)
    {
        return index < Count && index >=0 ? _boards[index] : null;
    }

    public IEnumerator<BoardData> GetEnumerator()
    {
        return _boards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

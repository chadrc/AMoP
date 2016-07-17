using UnityEngine;
using System.Collections.Generic;

public class BoardData : ScriptableObject
{
    [SerializeField]
    private string boardName;
    public string BoardName { get { return boardName; } set { boardName = value; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } set { description = value; } }

    [SerializeField]
    [Range(3, 6)]
    private int boardSize = 6;
    public int BoardSize { get { return boardSize; } set { if (value >= 3 && value <= 6) { boardSize = value; } } }

    [SerializeField]
    private List<BoardNodeData> nodes = new List<BoardNodeData>();
    public List<BoardNodeData> Nodes { get { return new List<BoardNodeData>(nodes); } }

    public int MaxNodes { get { return boardSize * boardSize * boardSize; } }
    public float OffsetValue { get { return (BoardSize / 2f) - .5f; } }

    public void AddNode()
    {
        if (nodes.Count < MaxNodes)
        {
            nodes.Add(new BoardNodeData());
        }
        else
        {
            throw new System.InvalidOperationException("Cannot have more than " + MaxNodes + " nodes in this board.");
        }
    }

    public void RemoveNode(BoardNodeData node)
    {
        nodes.Remove(node);
    }

	public void RemoveNode(int index)
	{
		nodes.RemoveAt (index);
	}
}
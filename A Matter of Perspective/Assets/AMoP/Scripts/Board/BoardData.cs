using UnityEngine;
using System.Collections.Generic;

public enum BoardCompletionLevel : int
{
    Completed = -1,
    Bronze = 0,
    Silver = 1,
    Gold = 2,
}

[System.Serializable]
public class BoardScores
{
    [SerializeField]
    private int[] scores = new int[3];

    public int this[BoardCompletionLevel level]
    {
        get
        {
            return scores[(int)level];
        }
    }

    public int HighestScore
    {
        get
        {
            return scores[2];
        }
    }

    public BoardCompletionLevel GetCompletionLevel(int score)
    {
        int highest = -1;
        int current = 0;
        foreach (var s in scores)
        {
            if (score >= s)
            {
                highest = current;
                current++;
            }
            else
            {
                break;
            }
        }
        return (BoardCompletionLevel)highest;
    }
}

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

    [SerializeField]
    private BoardScores scores = new BoardScores();
    public BoardScores Scores { get { return scores; } }

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
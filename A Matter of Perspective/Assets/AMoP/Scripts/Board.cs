using UnityEngine;
using System.Collections.Generic;

public class Board
{
    private List<BoardNode> nodes = new List<BoardNode>();

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<BoardNode> Nodes
    {
        get
        {
            return new List<BoardNode>(nodes);
        }
    }

    public Board(BoardData data, BoardNodeFactory nodeFactory)
    {
        foreach (var nodeData in data.Nodes)
        {
            nodes.Add(nodeFactory.CreateNode(nodeData));
        }
    }
}

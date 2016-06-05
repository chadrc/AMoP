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
    public BoardBehavior Behavior { get; private set; }

    public Board(BoardData data, BoardBehavior behavior, BoardNodeFactory nodeFactory)
    {
        Behavior = behavior;
        foreach (var nodeData in data.Nodes)
        {
            BoardNode node = nodeFactory.CreateNode(nodeData);
            node.Behavior.transform.SetParent(behavior.transform);
            nodes.Add(node);
        }
    }
}

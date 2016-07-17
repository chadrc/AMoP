using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Board : IEnumerable<BoardNode>
{
    private List<BoardNode> nodes = new List<BoardNode>();
    private BoardNodeFactory nodeFactory;

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
    public int BoardSize { get; private set; }
    public float OffsetValue
    {
        get
        {
            return (BoardSize / 2f) - 0.5f;
        }
    }
    public Vector3 OffsetVector
    {
        get
        {
            return new Vector3(OffsetValue, OffsetValue, OffsetValue);
        }
    }

    public Board(BoardData data, BoardBehavior behavior, BoardNodeFactory nodeFactory)
    {
        BoardSize = data.BoardSize;
        this.nodeFactory = nodeFactory;
        Behavior = behavior;
        foreach (var nodeData in data.Nodes)
        {
            makeNode(nodeData);
        }
    }

    public void ReplaceNode(BoardNode original, BoardNodeData newData)
    {
        nodes.Remove(original);
        makeNode(newData);
    }

    public void RemoveNode(BoardNode node)
    {
        nodes.Remove(node);
    }

    public BoardNode GetNode(int x, int y)
    {
        return GetNodeRow(x, y).Closest;
    }

    public List<BoardNode> GetNotNodes(int x, int y)
    {
        return GetNodeRow(x, y).Hidden;
    }

    public NodeRow GetNodeRow(int x, int y)
    {
		return AMoPUtils.GetNodeRow (Nodes, (int)(x - OffsetValue), (int)(y - OffsetValue));
    }

    public IEnumerator<BoardNode> GetEnumerator()
    {
        return nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void makeNode(BoardNodeData data)
    {
        BoardNode node = nodeFactory.CreateNode(data, this);
        nodes.Add(node);
    }
}

public class NodeRow
{
    public readonly BoardNode Closest;
    public readonly List<BoardNode> Hidden;

    public NodeRow(BoardNode closest, List<BoardNode> hidden)
    {
        Closest = closest;
        Hidden = hidden;
    }
}
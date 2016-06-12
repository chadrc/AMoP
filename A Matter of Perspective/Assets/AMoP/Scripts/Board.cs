using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Board : IEnumerable<BoardNode>
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
            node.SetBoard(this);
            nodes.Add(node);
        }
        behavior.Init(this);
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
        float posX = x - 2.5f;
        float posY = y - 2.5f;

        List<BoardNode> xyNodes = new List<BoardNode>();
        foreach (var node in Nodes)
        {
            // Check for position in 1 unit range to account for float errors
            if (node.Behavior.transform.position.x > posX - .5f && node.Behavior.transform.position.x < posX + .5f &&
                node.Behavior.transform.position.y > posY - .5f && node.Behavior.transform.position.y < posY + .5f)
            {
                xyNodes.Add(node);
            }
        }

        BoardNode matchNode = null;

        foreach (var node in xyNodes)
        {
            if (matchNode == null || matchNode.Behavior.transform.position.z > node.Behavior.transform.position.z)
            {
                matchNode = node;
            }
        }

        if (matchNode != null)
        {
            xyNodes.Remove(matchNode);
        }

        return new NodeRow(matchNode, xyNodes);
    }

    public IEnumerator<BoardNode> GetEnumerator()
    {
        return nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
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
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
    private List<BoardNodeData> nodes = new List<BoardNodeData>();
    public List<BoardNodeData> Nodes { get { return new List<BoardNodeData>(nodes); } }

    public void AddNode()
    {
        if (nodes.Count < 6 * 6 * 6)
        {
            nodes.Add(new BoardNodeData());
        }
        else
        {
            throw new System.InvalidOperationException("Cannot have more than " + 6 * 6 * 6 + " nodes in a board.");
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

public enum BoardNodeType
{
    Basic,
    Pool,
    Vortex,
    Block,
    Moving,
    Null,
}

public enum BoardNodeAffiliation
{
    Neutral,
    Player,
    Enemy
}

[System.Serializable]
public class BoardNodeData
{
    [SerializeField]
    private Vector3 position = new Vector3(0,0,0);
    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            if (value.x >= 0 && value.x < 6
                && value.y >= 0 && value.y < 6
                && value.z >= 0 && value.z < 6)
            {
                position = value;
            }
        }
    }

    [SerializeField]
    private BoardNodeType type = BoardNodeType.Basic;
    public BoardNodeType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    [SerializeField]
    private float startingEnergy;
    public float StartingEnergy
    {
        get
        {
            return startingEnergy;
        }

        set
        {
            if (value >= 0)
            {
                startingEnergy = value;
            }
        }
    }

    [SerializeField]
    private BoardNodeAffiliation affiliation = BoardNodeAffiliation.Neutral;
    public BoardNodeAffiliation Affiliation
    {
        get
        {
            return affiliation;
        }

        set
        {
            affiliation = value;
        }
    }
}
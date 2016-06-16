using UnityEngine;
using System.Collections.Generic;

public class BoardData : ScriptableObject
{
    [SerializeField]
    private string boardName;
    public string BoardName { get { return boardName; } }

    [SerializeField]
    private string description;
    public string Description { get { return description; } }

    [SerializeField]
    private List<BoardNodeData> nodes = new List<BoardNodeData>();
    public List<BoardNodeData> Nodes { get { return new List<BoardNodeData>(nodes); } }
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
    public Vector3 Position { get { return position; } }

    [SerializeField]
    private BoardNodeType type = BoardNodeType.Basic;
    public BoardNodeType Type { get { return type; } }

    [SerializeField]
    private float startingEnergy;
    public float StartingEnergy { get { return startingEnergy; } }

    [SerializeField]
    private BoardNodeAffiliation affiliation = BoardNodeAffiliation.Neutral;
    public BoardNodeAffiliation Affiliation { get { return affiliation; } }
}
using UnityEngine;

public enum BoardNodeType
{
    Basic,
    Drain,
    Fill,
    Null,
    Pool,
    Redirect,
    Vortex,
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
    private Vector3 position = new Vector3(0, 0, 0);
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

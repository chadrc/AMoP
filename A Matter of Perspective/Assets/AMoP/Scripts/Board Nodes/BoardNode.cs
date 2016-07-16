using UnityEngine;
using System.Collections;

public abstract class BoardNode
{
    private Coroutine updateRoutine;
    
    public BoardNodeBehavior Behavior { get; private set; }
    public Board ParentBoard { get; private set; }
    public Vector2 PerspectivePos
    {
        get
        {
            return new Vector2(Behavior.transform.position.x, Behavior.transform.position.y);
        }
    }

    public Property<Vector3> Position { get; protected set; }
    public Property<BoardNodeType> Type { get; protected set; }
    public Property<BoardNodeAffiliation> Affiliation { get; protected set; }
    public Property<float> Energy { get; protected set; }

    public abstract bool CanSend { get; }
    public abstract bool CanReceive { get; }

    protected float MaxEnergy = GameData.Constants.NodeMaxEnergy;

    public BoardNode(BoardNodeData data)
    {
        Position = data.Position;
        Energy = data.StartingEnergy;
        Type = data.Type;
        Affiliation = data.Affiliation;
    }

    ~BoardNode()
    {
        DetachFromBehavior();
    }

    public void Enable()
    {
        Behavior.EnergyCollider.enabled = true;
    }

    public void Disable()
    {
        Behavior.EnergyCollider.enabled = false;
    }

    public void SetBoard(Board board)
    {
        ParentBoard = board;
        Behavior.transform.SetParent(board.Behavior.transform);
    }

    public void AttachedToBehavior(BoardNodeBehavior behavior)
    {
        DetachFromBehavior();

        Behavior = behavior;
        behavior.EnergyEnter += OnEnergyEnter;
        updateRoutine = behavior.StartCoroutine(UpdateRoutine());
    }

    public void DetachFromBehavior()
    {
        if (Behavior == null)
        {
            return;
        }

        Behavior.EnergyEnter -= OnEnergyEnter;
        Behavior.StopCoroutine(updateRoutine);
        Behavior = null;
    }

    public void SendEnergy(BoardNode to)
    {
        if (CanSend && to.CanReceive)
        {
            Behavior.StartCoroutine(DoSendEnergy(to));
        }
    }

    protected virtual void OnEnergyEnter(EnergyBehavior energyBehavior)
    {
        if (energyBehavior.EnergyObj.Affiliation == Affiliation.Value || Affiliation.Value == BoardNodeAffiliation.Neutral)
        {
            Affiliation.Value = energyBehavior.EnergyObj.Affiliation;
            float newValue = Energy + 1;
            if (newValue <= 20f)
            {
                Energy.Value = newValue;
            }
        }
        else
        {
            float newValue = Energy - 1;
            if (newValue >= 0)
            {
                Energy.Value = newValue;
                if (newValue == 0)
                {
                    Affiliation.Value = BoardNodeAffiliation.Neutral;
                }
            }
        }
    }

    private IEnumerator DoSendEnergy(BoardNode to)
    {
        int toSend = Mathf.RoundToInt(Energy.Value);
        var range = new Range(toSend);
        #pragma warning disable 0168
        foreach (var i in range)
        {
            Behavior.SendEnergy(to);
            Energy.Value--;
            yield return new WaitForSeconds(.1f);
        }
        #pragma warning restore 0168
    }

    private IEnumerator UpdateRoutine()
    {
        while (true)
        {
            Update();
            yield return new WaitForEndOfFrame();
        }
    }

    protected abstract void Update();

    protected static void DeferEnergyEnterTo(BoardNode node, EnergyBehavior energy)
    {
        node.OnEnergyEnter(energy);
    }
}

using UnityEngine;
using System.Collections;

public class BoardNode
{
    private Coroutine updateRoutine;
    
    public BoardNodeBehavior Behavior { get; private set; }
    public Board ParentBoard { get; private set; }

    public Property<Vector3> Position { get; private set; }
    public Property<BoardNodeType> Type { get; private set; }
    public Property<BoardNodeAffiliation> Affiliation { get; private set; }
    public Property<float> Energy { get; private set; }

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

    private IEnumerator UpdateRoutine()
    {
        while(true)
        {
            // Update Node

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnEnergyEnter(EnergyBehavior energyBehavior)
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

    public void SendEnergy(BoardNode to)
    {
        Behavior.StartCoroutine(DoSendEnergy(to));
    }

    IEnumerator DoSendEnergy(BoardNode to)
    {
        while (Energy > 0)
        {
            var energy = LevelBehavior.Current.EnergyPoolManager.GetOneEnergy(Affiliation);
            energy.Travel(this, to);
            Energy.Value--;
            yield return new WaitForSeconds(.1f);
        }
    }
}

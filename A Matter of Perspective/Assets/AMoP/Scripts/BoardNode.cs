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

    /// <summary>
    /// Reduces energy by (int)energy.
    /// </summary>
    /// <returns>Returns amount of energy as int.</returns>
    public int Drain()
    {
        int amount = (int)Energy;
        Energy.Value -= amount;
        return amount;
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

    private void OnEnergyEnter(int amount)
    {

    }
}

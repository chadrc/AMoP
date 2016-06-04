using UnityEngine;
using System.Collections;

public class BoardNode
{
    private BoardNodeBehavior behavior;
    private Coroutine updateRoutine;
    
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

    public void AttachedToBehavior(BoardNodeBehavior behavior)
    {
        DetachFromBehavior();

        behavior.EnergyEnter += OnEnergyEnter;
        updateRoutine = behavior.StartCoroutine(UpdateRoutine());
    }

    public void DetachFromBehavior()
    {
        if (behavior == null)
        {
            return;
        }

        behavior.EnergyEnter -= OnEnergyEnter;
        behavior.StopCoroutine(updateRoutine);
        behavior = null;
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

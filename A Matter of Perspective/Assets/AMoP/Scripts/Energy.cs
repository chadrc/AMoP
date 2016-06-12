using UnityEngine;
using System.Collections;

public class Energy
{
    public EnergyBehavior Behavior { get; private set; }
    public BoardNodeAffiliation Affiliation { get; private set; }
    public BoardNode Origin { get; private set; }
    public BoardNode Destination { get; private set; }

    public Energy(EnergyBehavior behavior)
    {
        Behavior = behavior;
    }

    public void Travel(BoardNode from, BoardNode to)
    {
        Affiliation = from.Affiliation;
        Origin = from;
        Destination = to;
        Behavior.transform.position = from.Behavior.transform.position;
        Behavior.TravelTo(to);
    }
}

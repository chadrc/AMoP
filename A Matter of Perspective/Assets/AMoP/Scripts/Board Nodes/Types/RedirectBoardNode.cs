using UnityEngine;

public class RedirectBoardNode : BoardNode
{
    public RedirectBoardNode(BoardNodeData data) : base(data)
    {
    }

    public override bool CanReceive
    {
        get
        {
            return true;
        }
    }

    public override bool CanSend
    {
        get
        {
            return false;
        }
    }

    protected override void Update()
    {

    }

    protected override void OnEnergyEnter(EnergyBehavior energyBehavior)
    {
        var row = ParentBoard.GetNotNodes((int)(Behavior.transform.position.x+2.5f), (int)(Behavior.transform.position.y+2.5f));
        BoardNode closest = null;
        if (row.Count == 1)
        {
            closest = row[0];
        } 
        else
        {
            float closestDist = float.MaxValue;
            foreach (var n in row)
            {
                float dist = Vector3.Distance(Behavior.transform.position, n.Behavior.transform.position);
                if (closest == null || dist < closestDist)
                {
                    closestDist = dist;
                    closest = n;
                }
            }
        }

        if (closest != null)
        {
            DeferEnergyEnterTo(closest, energyBehavior);
        }
    }
}

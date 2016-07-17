using UnityEngine;

public class DrainBoardNode : BoardNode
{
    float depletionRate = GameData.Constants.DrainNodeDepletionRate;

    public DrainBoardNode(BoardNodeData data, Board parent) : base(data, parent)
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
            return true;
        }
    }

    protected override void Update()
    {
        float energy = Energy;
        energy -= Time.deltaTime * depletionRate;
        if (energy < 0)
        {
            energy = 0;
        }
        Energy.Value = energy;
    }
}

using UnityEngine;
using System.Collections;
using System;

public class DrainBoardNode : BoardNode
{
    float depletionRate = .5f;

    public DrainBoardNode(BoardNodeData data) : base(data)
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

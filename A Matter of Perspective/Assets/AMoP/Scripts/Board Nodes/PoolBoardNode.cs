using UnityEngine;
using System.Collections;
using System;

public class PoolBoardNode : BoardNode
{
    private float regenRate = 1.0f;

    public PoolBoardNode(BoardNodeData data) : base(data)
    {

    }

    public override bool CanReceive
    {
        get
        {
            return false;
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
        energy += Time.deltaTime * regenRate;
        if (energy > MaxEnergy)
        {
            energy = MaxEnergy;
        }
        Energy.Value = energy;
    }
}

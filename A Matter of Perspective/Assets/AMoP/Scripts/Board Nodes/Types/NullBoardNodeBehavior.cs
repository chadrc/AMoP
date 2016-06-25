using UnityEngine;
using System.Collections;
using System;

public class NullBoardNodeBehavior : BoardNodeBehavior
{
    [SerializeField]
    new private MeshRenderer renderer;

    public override void SendEnergy(BoardNode to)
    {
    }

    protected override void Awake()
    {
    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
    }

    protected override void OnNodeEnergyChanged(float energy)
    {
    }

    protected override void OnNodeTypeChanged(BoardNodeType type)
    {
    }

    protected override void setAlpha(float a)
    {
        var clr = renderer.material.color;
        clr.a = a;
        renderer.material.color = clr;
    }
}

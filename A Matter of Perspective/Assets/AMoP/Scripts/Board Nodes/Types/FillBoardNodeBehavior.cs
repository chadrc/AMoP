using UnityEngine;
using System.Collections;
using System;

public class FillBoardNodeBehavior : BoardNodeBehavior
{
    [SerializeField]
    private GameObject graphicObject;
    new private MeshRenderer renderer;

    private Vector3 minScale = new Vector3(.25f, .25f, .25f);
    private Vector3 maxScale = new Vector3(.75f, .75f, .75f);

    protected override void Awake()
    {
        renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public override void SendEnergy(BoardNode to)
    {

    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        renderer.material.color = AMoPUtils.GetColorForAffiliation(affiliation);
    }

    protected override void OnNodeEnergyChanged(float energy)
    {
        graphicObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Node.Energy / 20f);
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

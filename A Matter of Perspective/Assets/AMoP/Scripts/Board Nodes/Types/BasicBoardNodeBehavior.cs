using UnityEngine;

public class BasicBoardNodeBehavior : BoardNodeBehavior
{
    [SerializeField]
    private GameObject graphicObject;
    new private MeshRenderer renderer;

    private Vector3 minScale = new Vector3(.25f, .25f, .25f);
    private Vector3 maxScale = new Vector3(.75f, .75f, .75f);

    public override void SendEnergy(BoardNode to)
    {
        var energy = LevelBehavior.Current.EnergyPoolManager.GetOneEnergy(Node.Affiliation);
        energy.Travel(Node, to);
    }

    protected override void Awake()
    {
        renderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    protected override void OnNodeTypeChanged(BoardNodeType type)
    {
    }

    protected override void OnNodeEnergyChanged(float energy)
    {
        graphicObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Node.Energy / 20f);
    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        renderer.material.color = AMoPUtils.GetColorForAffiliation(affiliation);
        if (Hidden)
        {
            setAlpha(0);
        }
    }

    protected override void setAlpha(float a)
    {
        var clr = renderer.material.color;
        clr.a = a;
        renderer.material.color = clr;
    }
}

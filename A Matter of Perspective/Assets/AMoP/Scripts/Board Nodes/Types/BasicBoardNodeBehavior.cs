using UnityEngine;

public class BasicBoardNodeBehavior : BoardNodeBehavior
{
    [SerializeField]
    private GameObject graphicObject;
    new private MeshRenderer renderer;

    private Vector3 minScale = new Vector3(.25f, .25f, .25f);
    private Vector3 maxScale = new Vector3(.75f, .75f, .75f);

    public override void AttachToNode(BoardNode node)
    {
        base.AttachToNode(node);
        Resize();
        ChangeColor();
    }

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
        Resize();
    }

    protected override void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        ChangeColor();
    }

    protected override void ChangeColor()
    {
        switch (Node.Affiliation.Value)
        {
            case BoardNodeAffiliation.Player:
                renderer.material.color = Color.cyan;
                break;

            case BoardNodeAffiliation.Enemy:
                renderer.material.color = new Color(1.0f, 1.0f, 0);
                break;

            case BoardNodeAffiliation.Neutral:
                renderer.material.color = Color.white;
                break;
        }
    }

    protected override void setAlpha(float a)
    {
        var clr = renderer.material.color;
        clr.a = a;
        renderer.material.color = clr;
    }

    private void Resize()
    {
        graphicObject.transform.localScale = Vector3.Lerp(minScale, maxScale, Node.Energy / 20f);
    }
}

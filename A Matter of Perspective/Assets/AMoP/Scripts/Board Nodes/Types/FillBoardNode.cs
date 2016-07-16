
public class FillBoardNode : BoardNode
{
    public FillBoardNode(BoardNodeData data) : base(data)
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
        base.OnEnergyEnter(energyBehavior);
        if (Energy == GameData.Constants.FillNodeFillRequirement)
        {
            ParentBoard.RemoveNode(this);
            Behavior.gameObject.SetActive(false);
        }
    }
}

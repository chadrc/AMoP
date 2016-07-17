
public class NullBoardNode : BoardNode
{
    public NullBoardNode(BoardNodeData data, Board parent) : base(data, parent)
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
            return false;
        }
    }

    protected override void Update()
    {
    }
}

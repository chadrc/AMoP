﻿
public class BasicBoardNode : BoardNode
{

    public BasicBoardNode(BoardNodeData data, Board parent) : base(data, parent)
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

    }
}

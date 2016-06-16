using UnityEngine;
using System.Collections;
using System;

public class BasicBoardNode : BoardNode
{

    public BasicBoardNode(BoardNodeData data) : base(data)
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

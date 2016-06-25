using UnityEngine;
using System.Collections;
using System;

public class NullBoardNode : BoardNode
{
    public NullBoardNode(BoardNodeData data) : base(data)
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

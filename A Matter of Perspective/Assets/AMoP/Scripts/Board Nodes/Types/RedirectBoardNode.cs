﻿using UnityEngine;
using System.Collections;
using System;

public class RedirectBoardNode : BoardNode
{
    public RedirectBoardNode(BoardNodeData data) : base(data)
    {
    }

    public override bool CanReceive
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public override bool CanSend
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    protected override void Update()
    {
        throw new NotImplementedException();
    }
}
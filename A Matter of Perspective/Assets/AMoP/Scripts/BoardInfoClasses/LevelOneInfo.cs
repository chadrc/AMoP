using UnityEngine;
using System.Collections;

public class LevelOneInfo : BaseBoardInfo
{
    protected override void setup()
    {
        infoController.Highlight(0, 0);
        infoController.SetText("This is a node, it stores energy. Touch and drag from a full node to an empty one to transfer the energy.");
    }
}

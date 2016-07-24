using UnityEngine;
using System.Collections;

public class LevelOneInfo : BaseBoardInfo
{
    protected override void setup()
    {
        infoController.Highlight(0, 0, Color.red);
    }
}

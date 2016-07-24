using UnityEngine;
using System.Collections;
using System;

public class LevelOneInfo : BaseBoardInfo
{
    BoardInfoHighlighter highlighter;

    protected override void setup()
    {
        highlighter = infoController.Highlight(0, 0);
        infoController.SetText("This is a node, it stores energy. Touch and drag from a full node to an empty one to transfer the energy.");
    }

    protected override void destroy()
    {
        GameObject.Destroy(highlighter.gameObject);
        infoController.UnsetText();
    }
}

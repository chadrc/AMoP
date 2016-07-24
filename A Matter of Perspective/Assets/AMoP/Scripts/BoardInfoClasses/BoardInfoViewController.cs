using UnityEngine;
using System.Collections;

public class BoardInfoViewController : MonoBehaviour
{
    [SerializeField]
    private NodeButtonPanelViewController nodeButtonController;

    [SerializeField]
    private GameObject highlighterPrefab;

    public void Highlight(int x, int y, Color color)
    {
        var button = nodeButtonController.GetButton(x, y);
        var highlighter = createHighlighter();
        highlighter.Init(button);
        highlighter.SetColor(color);
    }

    public void Highlight(int x, int y)
    {
        Highlight(x, y, Color.green);
    }

    private BoardInfoHighlighter createHighlighter()
    {
        var obj = GameObject.Instantiate(highlighterPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localScale = Vector3.one;
        return obj.GetComponent<BoardInfoHighlighter>();
    }
}

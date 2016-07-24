using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoardInfoViewController : MonoBehaviour
{
    [SerializeField]
    private NodeButtonPanelViewController nodeButtonController;

    [SerializeField]
    private RectTransform boardInfoTextPanel;

    [SerializeField]
    private Text boardInfoText;

    [SerializeField]
    private GameObject highlighterPrefab;

    void Awake()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
    }

    public void SetText(string text)
    {
        boardInfoText.text = text;
    }

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

    private void onScreenChanged(int width, int height)
    {
        if (width > height)
        {
            boardInfoTextPanel.anchorMin = new Vector2(.8f, 0);
            boardInfoTextPanel.anchorMax = new Vector2(1, .75f);
        }
        else
        {
            boardInfoTextPanel.anchorMin = new Vector2(.25f, 0);
            boardInfoTextPanel.anchorMax = new Vector2(1, .2f);
        }
    }
}

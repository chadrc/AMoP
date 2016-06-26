using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeButtonBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Image graphic;

    [SerializeField]
    private Text energyText;

    public NodeButtonPanelViewController Controller { get; private set; }
    public int XIndex { get; private set; }
    public int YIndex { get; private set; }

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        LevelBehavior.GameStart += onGameStart;
    }

    public void Init(NodeButtonPanelViewController controller, int xIndex, int yIndex)
    {
        Controller = controller;
        XIndex = xIndex;
        YIndex = yIndex;

        var rectTransform = graphic.transform as RectTransform;
        float heightRatio = 1.0f / (Camera.main.orthographicSize * 2);
        float pixelHeight = Screen.height * heightRatio;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelHeight);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pixelHeight);
    }

    private void onGameStart()
    {
        var node = LevelBehavior.Current.CurrentBoard.GetNode(XIndex, YIndex);
        if (node != null)
        {
            node.Energy.Changed += onNodeEnergyChanged;
            energyText.text = ((int)node.Energy.Value).ToString();
        }
    }

    private void onNodeEnergyChanged(float value)
    {
        energyText.text = ((int)value).ToString();
    }

    private void Show()
    {
        canvasGroup.alpha = 1.0f;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
    }

    public void Select()
    {
        graphic.color = new Color(0, 1.0f, 0, .5f);
        Show();
    }

    public void Deselect()
    {
        Hide();
    }

    public void Hover()
    {
        graphic.color = new Color(1.0f, 0, 1.0f, .5f);
        Show();
    }

    public void Unhover()
    {
        Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Controller.ButtonDown(this, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Controller.ButtonUp(this, eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Controller.ButtonEnter(this, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Controller.ButtonExit(this, eventData);
    }
}

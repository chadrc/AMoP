using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class BoardInfoHighlighter : MonoBehaviour
{
    [SerializeField]
    private Image highlighterImage;

    private RectTransform rectTransform;
    private RectTransform imageRectTransform;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        imageRectTransform = highlighterImage.GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();
    }

    public void Init(NodeButtonBehavior button)
    {
        rectTransform.anchoredPosition = button.Position;
        imageRectTransform.SetSize(button.Size);
        canvasGroup.Show();
        canvasGroup.interactable = false;
    }

    public void SetColor(Color color)
    {
        highlighterImage.color = color;
    }
}

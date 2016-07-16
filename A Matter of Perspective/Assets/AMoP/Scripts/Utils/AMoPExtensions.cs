using UnityEngine;

public static class AMoPExtensions
{
    public static void Hide(this CanvasGroup canvasGroup)
    {
        canvasGroup.SetAlpha(0);
    }

    public static void Show(this CanvasGroup canvasGroup)
    {
        canvasGroup.SetAlpha(1.0f);
    }

    public static void SetAlpha(this CanvasGroup canvasGroup, float alpha)
    {
        canvasGroup.alpha = alpha;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = (alpha > 0);
    }
}

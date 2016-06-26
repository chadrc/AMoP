using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class MenuViewController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        LevelBehavior.GameEnd += OnGameEnd;
    }

    private void OnDestroy()
    {
        LevelBehavior.GameEnd -= OnGameEnd;
    }

    public void StartButtonPressed()
    {
        LevelBehavior.Current.InitBoard();
        canvasGroup.gameObject.SetActive(false);
    }

    private void OnGameEnd()
    {
        canvasGroup.gameObject.SetActive(true);
    }
}

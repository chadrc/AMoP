using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class MenuViewController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.Show();
    }

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
        LevelBehavior.Current.StartGame(0, 0);
        canvasGroup.Hide();
    }

    private void OnGameEnd()
    {

    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class MenuViewController : MonoBehaviour
{
    [SerializeField]
    private OptionsViewController optionsViewController;

    [SerializeField]
    private CanvasGroup storeCanvasGroup;

    [SerializeField]
    private VerticalLayoutGroup buttonLayoutGroup;

    private CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.Show();
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Show();
        LevelBehavior.GameEnd += OnGameEnd;
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
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

    public void OptionsButtonPressed()
    {
        canvasGroup.Hide();
        optionsViewController.Show(canvasGroup);
    }

    public void StoreButtonPressed()
    {
        canvasGroup.Hide();
        storeCanvasGroup.Show();
    }

    private void OnGameEnd()
    {

    }

    private void onScreenChanged(int width, int height)
    {
        if (width > height)
        {
            buttonLayoutGroup.padding = new RectOffset(400, 400, 100, 100);
            buttonLayoutGroup.spacing = 50;
        }
        else
        {
            buttonLayoutGroup.padding = new RectOffset(150, 150, 200, 200);
            buttonLayoutGroup.spacing = 100;
        }
    }
}

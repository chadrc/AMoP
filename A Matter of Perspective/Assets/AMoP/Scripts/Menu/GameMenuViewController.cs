using UnityEngine;
using UnityEngine.UI;

public class GameMenuViewController : MonoBehaviour
{
    [SerializeField]
    private MenuViewController menuViewController;

    [SerializeField]
    private CanvasGroup pausePanel;

    [SerializeField]
    private GridLayoutGroup gridLayout;

    [SerializeField]
    private CanvasGroup inGamePanel;

    [SerializeField]
    private CanvasGroup optionsPanel;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();

        LevelBehavior.GameStart += onGameStart;
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
    }

    void OnDestroy()
    {
        LevelBehavior.GameStart -= onGameStart;
        ScreenChangeListeningBehavior.ScreenChanged -= onScreenChanged;
    }

    public void OnPauseButtonPressed()
    {
        pausePanel.Show();
        inGamePanel.Hide();
    }

    public void OnMenuButtonPressed()
    {
        menuViewController.Show();
        canvasGroup.Hide();
    }

    public void OnResumeButtonPressed()
    {
        pausePanel.Hide();
        inGamePanel.Show();
    }

    public void OnRestartButtonPressed()
    {
        canvasGroup.Hide();
        LevelBehavior.Current.StartGame();
    }

    public void OnOptionsButtonPressed()
    {

    }

    private void onGameStart()
    {
        canvasGroup.Show();
        inGamePanel.Show();
        pausePanel.Hide();
    }

    private void onScreenChanged(int width, int height)
    {
        gridLayout.constraintCount = width > height ? 2 : 1;
    }
}

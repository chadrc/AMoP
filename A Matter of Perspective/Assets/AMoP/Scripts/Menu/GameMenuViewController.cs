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
    private CanvasGroup nodeDisplay;

    [SerializeField]
    private Text nodesText;

    [SerializeField]
    private OptionsViewController optionsViewController;

    private CanvasGroup canvasGroup;

    private int totalNodes;
    private int captureCount;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();
        pausePanel.Hide();

        LevelBehavior.GameStart += onGameStart;
        LevelBehavior.GameEnd += onGameEnd;
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
        canvasGroup.Hide();
        optionsViewController.Show(canvasGroup);
    }

    private void updateNodesText()
    {
        nodesText.text = captureCount + "/" + totalNodes;
    }

    private void onGameEnd()
    {
        inGamePanel.Hide();
    }

    private void onGameStart()
    {
        canvasGroup.Show();
        inGamePanel.Show();
        pausePanel.Hide();

        captureCount = 0;

        var board = LevelBehavior.Current.CurrentBoard;
        totalNodes = board.CapturableNodeCount;
        foreach(var node in board)
        {
            node.Affiliation.Changed += onNodeAffiliationChanged;
            if (node.Affiliation == BoardNodeAffiliation.Player && node.CanReceive)
            {
                captureCount++;
            }
        }
        updateNodesText();
    }

    private void onScreenChanged(int width, int height)
    {
    }

    private void onNodeAffiliationChanged(BoardNodeAffiliation affliation)
    {
        if (affliation == BoardNodeAffiliation.Player)
        {
            captureCount++;
        }
        else
        {
            captureCount--;
        }

        updateNodesText();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(CanvasGroup))]
public class EndGamePanelViewController : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelButton;

    [SerializeField]
    private Text gameTimeText;

    [SerializeField]
    private Text boardTurnsText;

    [SerializeField]
    private Text energyTransfersText;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private GridLayoutGroup buttonGrid;

    [SerializeField]
    private MenuViewController menu;

    private CanvasGroup canvasGroup;

    private float timeTillPanelShows = 1.0f;
    private float fadeInTime = .5f;
    private float timeForGameTimeCountUp = 1.0f;

	// Use this for initialization
	void Awake ()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        LevelBehavior.GameEnd += OnGameEnd;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.Hide();
    }

    void OnDestroy()
    {
        LevelBehavior.GameEnd -= OnGameEnd;
    }

    public void MenuButtonPress()
    {
        menu.Show();
        canvasGroup.Hide();
    }

    public void ReplayButtonPress()
    {
        canvasGroup.Hide();
        LevelBehavior.Current.StartGame();
    }

    public void NextButtonPress()
    {
        canvasGroup.Hide();
        if (LevelBehavior.Current.AdvanceToNextLevel())
        {
            LevelBehavior.Current.StartGame();
        }
        else
        {
            Debug.LogError("Cannot advance to next level or series.");
        }
    }

    private void onScreenChanged(int width, int height)
    {
        buttonGrid.constraint = width >= height ? GridLayoutGroup.Constraint.FixedRowCount : GridLayoutGroup.Constraint.FixedColumnCount;
    }

    private void OnGameEnd()
    {
        canvasGroup.Hide();
        if (!LevelBehavior.Current.HasNextLevel && !LevelBehavior.Current.HasNextSeries)
        {
            nextLevelButton.gameObject.SetActive(false);
        }
        else
        {
            nextLevelButton.gameObject.SetActive(true);
        }
        StartCoroutine(showRoutine());
    }

    private IEnumerator showRoutine()
    {
        float timer=0;

        // Slowing time effect
        while(timer < GameData.Constants.EndAnimationSlowDownEffectTime)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = 1.0f - Mathf.Clamp01(timer / timeTillPanelShows);
            yield return new WaitForEndOfFrame();
        }
        
        // Fade in panel
        timer = 0;
        while (timer < GameData.Constants.EndAnimationPanelFadeInTime)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.SetAlpha(timer / fadeInTime);
            yield return new WaitForEndOfFrame();
        }
        
        // Count up animations for score values
        timer = 0;
        while (timer < GameData.Constants.EndAnimationTextAnimationTime)
        {
            float frac = (timer / timeForGameTimeCountUp);
            displayStats(frac);
            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        // Make sure final numbers are shown
        displayStats(1.0f);

        // Slight delay
        yield return new WaitForSecondsRealtime(GameData.Constants.EndAnimationScoreDelay);

        // Count up animation for final score
        timer = 0;
        int score = LevelBehavior.Current.Score;
        while (timer < GameData.Constants.EndAnimationTextAnimationTime)
        {
            float frac = (timer / timeForGameTimeCountUp);
            float scoreDisplay = Mathf.Lerp(0, score, frac);
            scoreText.text = ((int)scoreDisplay).ToString();

            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        scoreText.text = score.ToString();
    }

    private void displayStats(float t)
    {
        // Game Time
        float timeDisplay = Mathf.Lerp(0, LevelBehavior.Current.GameTime, t);
        int seconds = (int)timeDisplay;
        int min = seconds / 60;
        seconds %= 60;

        int ms = (int)((timeDisplay - seconds) * 100f);

        string minStr = min.ToString("00");
        string secStr = seconds.ToString("00");
        string msStr = ms.ToString("00");

        string display;
        if (min > 0)
        {
            display = minStr + ":" + secStr + "." + msStr;
        }
        else
        {
            display = secStr + "." + msStr;
        }

        gameTimeText.text = display;

        // Board Turns
        float btDisplay = Mathf.Lerp(0, LevelBehavior.Current.BoardTurnCount, t);
        boardTurnsText.text = ((int)btDisplay).ToString();

        // Energy Transfers
        float etDisplay = Mathf.Lerp(0, LevelBehavior.Current.EnergyTransferCount, t);
        energyTransfersText.text = ((int)etDisplay).ToString();
    }
}

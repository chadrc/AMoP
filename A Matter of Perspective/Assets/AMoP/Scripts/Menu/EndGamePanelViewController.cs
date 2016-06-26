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
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OnDestroy()
    {
        LevelBehavior.GameEnd -= OnGameEnd;
    }

    public void MenuButtonPress()
    {
        menu.Show();
        Hide();
    }

    public void ReplayButtonPress()
    {
        Hide();
        LevelBehavior.Current.StartGame();
    }

    public void NextButtonPress()
    {
        Hide();
        if (LevelBehavior.Current.AdvanceToNextLevel())
        {
            LevelBehavior.Current.StartGame();
        }
        else
        {
            Debug.LogError("Cannot advance to next level or series.");
        }
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void onScreenChanged(int width, int height)
    {
        buttonGrid.constraint = width >= height ? GridLayoutGroup.Constraint.FixedRowCount : GridLayoutGroup.Constraint.FixedColumnCount;
    }

    private void OnGameEnd()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
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

        while(timer < timeTillPanelShows)
        {
            timer += Time.unscaledDeltaTime;
            Time.timeScale = 1.0f - Mathf.Clamp01(timer / timeTillPanelShows);
            yield return new WaitForEndOfFrame();
        }
        
        timer = 0;
        while (timer < fadeInTime)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = (timer / fadeInTime);
            yield return new WaitForEndOfFrame();
        }

        float timeDisplay = 0;
        timer = 0;
        while (timer < timeForGameTimeCountUp)
        {
            timeDisplay = Mathf.Lerp(0, LevelBehavior.Current.GameTime, (timer / timeForGameTimeCountUp));
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

            timer += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}

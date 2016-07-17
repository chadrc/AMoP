using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    public static LevelBehavior Current { get; private set; }
    public static event System.Action GameStart;
    public static event System.Action GameEnd;

    [SerializeField]
    private BoardBehavior boardBehavior;

    [SerializeField]
    private int boardSeriesIndex;

    [SerializeField]
    private int startingBoardIndex;

    [SerializeField]
    private BoardNodeFactory boardNodeFactory;

    [SerializeField]
    private EnergyFactory energyFactory;

    [SerializeField]
    private NodeButtonPanelViewController buttonController;
    
    private NodeButtonBehavior downButton;
    private bool playing = false;

    public BoardScores Scores { get; private set; }
    public Board CurrentBoard { get; private set; }
    public EnergyPoolManager EnergyPoolManager { get; private set; }
    public bool HasNextLevel
    {
        get
        {
            var boardSeries = GameData.SeriesList.GetSeries(boardSeriesIndex);
            return startingBoardIndex+1 < boardSeries.Count;
        }
    }
    public bool HasNextSeries { get { return boardSeriesIndex + 1 < GameData.SeriesList.Count; } }

    // Score metrics
    public float GameTime { get; private set; }
    public int BoardTurnCount { get; private set; }
    public int EnergyTransferCount { get; private set; }
    public int Score { get { return Mathf.RoundToInt(( gameTimeScore + boardTurnScore + energyTransferScore) * GameData.Constants.ScoreMultiplier); } }

    private float gameTimeScore
    {
        get
        {
            if (GameTime == 0)
            {
                return 0;
            }
            else
            {
                return GameData.Constants.GameTimeWeight / GameTime;
            }
        }
    }

    private float boardTurnScore
    {
        get
        {
            if (BoardTurnCount == 0)
            {
                return 0;
            }
            else
            {
                return GameData.Constants.BoardTurnsWeight / BoardTurnCount;
            }
        }
    }

    private float energyTransferScore
    {
        get
        {
            if (EnergyTransferCount == 0)
            {
                return 0;
            }
            else
            {
                return GameData.Constants.EnergyTransfersWeight / EnergyTransferCount;
            }
        }
    }

    public void StartGame(int seriesIndex, int boardIndex)
    {
        boardSeriesIndex = seriesIndex;
        startingBoardIndex = boardIndex;
        StartGame();
    }

    public void StartGame()
    {
        GameTime = 0;
        BoardTurnCount = 0;
        EnergyTransferCount = 0;
        Time.timeScale = 1.0f;
        EnergyPoolManager.HideAllEnergy();
        DestroyBoard();

        GameTime = 0;
        var boardSeries = GameData.SeriesList.GetSeries(boardSeriesIndex);
        if (boardSeries == null)
        {
            Debug.LogError("No series with index " + boardSeriesIndex);
            return;
        }
        var boardData = boardSeries.GetBoard(startingBoardIndex);
        if (boardData == null)
        {
            Debug.LogError("No board data in series " + boardSeriesIndex + " with index " + startingBoardIndex);
            return;
        }

        Scores = boardData.Scores;
        CurrentBoard = new Board(boardData, boardBehavior, boardNodeFactory);
        CurrentBoard.Behavior.Init(CurrentBoard);
        CurrentBoard.Behavior.SpinEnd += OnSpinEnd;
        foreach (var node in CurrentBoard)
        {
            node.Affiliation.Changed += OnNodeAffiliationChanged;
        }

        buttonController.Init(CurrentBoard);

        playing = true;
        if (GameStart != null)
        {
            GameStart();
        }
    }

    public bool AdvanceToNextLevel()
    {
        if (HasNextLevel)
        {
            startingBoardIndex++;
        }
        else if (HasNextSeries)
        {
            boardSeriesIndex++;
            startingBoardIndex = 0;
        }
        else
        {
            return false;
        }

        return true;
    }

    public void DestroyBoard()
    {
        if (CurrentBoard == null)
        {
            return;
        }

        foreach (var node in CurrentBoard)
        {
            node.Affiliation.Changed -= OnNodeAffiliationChanged;
            GameObject.Destroy(node.Behavior.gameObject);
        }
        CurrentBoard.Behavior.SpinEnd -= OnSpinEnd;
        CurrentBoard.Behavior.Uninit();
        CurrentBoard = null;
    }

    void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
        else
        {
            throw new System.Exception("Duplicate Singleton Creation.");
        }
    }

    void Start()
    {
        // UI Events
        EnergyPoolManager = new EnergyPoolManager(energyFactory);
        buttonController.NodeSwipeOccurred += OnNodeSwipeOccurred;
        buttonController.SwipeOccurred += OnSwipeOccurred;
    }

    void Update()
    {
        if (playing)
        {
            GameTime += Time.deltaTime;
        }
    }

    void OnDestory()
    {
        CurrentBoard.Behavior.SpinEnd -= OnSpinEnd;
    }

    void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        foreach (var node in CurrentBoard)
        {
            // If any node doesn't equal new affiliation, board still needs to be filled
            if (node.CanReceive && node.Affiliation != affiliation)
            {
                return;
            }
        }

        // Clean up resources
        playing = false;
        if (GameEnd != null)
        {
            GameEnd();
        }
    }

    void OnNodeSwipeOccurred(NodeButtonBehavior down, NodeButtonBehavior up, Vector2 dir)
    {
        var fromNode = CurrentBoard.GetOffsetNode(down.XIndex, down.YIndex);
        var toNode = CurrentBoard.GetOffsetNode(up.XIndex, up.YIndex);

        fromNode.SendEnergy(toNode);
        EnergyTransferCount++;
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        CurrentBoard.Behavior.Spin(dir);
        if (downButton != null)
        {
            downButton.Deselect();
            downButton = null;
        }
    }

    void OnSpinEnd()
    {
        BoardTurnCount++;
    }
}

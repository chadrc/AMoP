using UnityEngine;
using System;

public class LevelBehavior : MonoBehaviour
{
    public static LevelBehavior Current { get; private set; }
    public static event Action GameStart;
    public static event Action GameEnd;

    [SerializeField]
    private BoardBehavior _boardBehavior;

    [SerializeField]
    private int _boardSeriesIndex;

    [SerializeField]
    private int _startingBoardIndex;

    [SerializeField]
    private BoardNodeFactory _boardNodeFactory;

    [SerializeField]
    private EnergyFactory _energyFactory;

    [SerializeField]
    private NodeButtonPanelViewController _buttonController;
    
    private NodeButtonBehavior _downButton;
    private bool _playing;

    public BoardScores Scores { get; private set; }
    public Board CurrentBoard { get; private set; }
    public EnergyPoolManager EnergyPoolManager { get; private set; }
    public bool HasNextLevel
    {
        get
        {
            var boardSeries = GameData.SeriesList.GetSeries(_boardSeriesIndex);
            return !(boardSeries == null ||_startingBoardIndex+1 < boardSeries.Count);
        }
    }
    public bool HasNextSeries { get { return _boardSeriesIndex + 1 < GameData.SeriesList.Count; } }

    // Score metrics
    public float GameTime { get; private set; }
    public int BoardTurnCount { get; private set; }
    public int EnergyTransferCount { get; private set; }
    public int Score { get { return Mathf.RoundToInt(( GameTimeScore + BoardTurnScore + EnergyTransferScore) * GameData.Constants.ScoreMultiplier); } }

    private float GameTimeScore
    {
        get
        {
            if (Mathf.Approximately(GameTime, 0))
            {
                return 0;
            }
            return GameData.Constants.GameTimeWeight / GameTime;
        }
    }

    private float BoardTurnScore
    {
        get
        {
            if (BoardTurnCount == 0)
            {
                return 0;
            }
            return GameData.Constants.BoardTurnsWeight / BoardTurnCount;
        }
    }

    private float EnergyTransferScore
    {
        get
        {
            if (EnergyTransferCount == 0)
            {
                return 0;
            }
            return GameData.Constants.EnergyTransfersWeight / EnergyTransferCount;
        }
    }

    public void StartGame(int seriesIndex, int boardIndex)
    {
        _boardSeriesIndex = seriesIndex;
        _startingBoardIndex = boardIndex;
        StartGame();
    }

    public virtual void StartGame()
    {
        ResetState();

        var boardSeries = GameData.SeriesList.GetSeries(_boardSeriesIndex);
        if (boardSeries == null)
        {
            Debug.LogError("No series with index " + _boardSeriesIndex);
            return;
        }
        var boardData = boardSeries.GetBoard(_startingBoardIndex);
        if (boardData == null)
        {
            Debug.LogError("No board data in series " + _boardSeriesIndex + " with index " + _startingBoardIndex);
            return;
        }

        SetUpBoard(boardData);
        DoStartGame();
        InitializeBoardInfoClass(boardData);
    }

    protected void ResetState()
    {
        GameTime = 0;
        BoardTurnCount = 0;
        EnergyTransferCount = 0;
        Time.timeScale = 1.0f;
        EnergyPoolManager.HideAllEnergy();
        DestroyBoard();
    }

    protected void SetUpBoard(BoardData boardData)
    {
        Scores = boardData.Scores;
        CurrentBoard = new Board(boardData, _boardBehavior, _boardNodeFactory);
        CurrentBoard.Behavior.Init(CurrentBoard);
        CurrentBoard.Behavior.SpinEnd += OnSpinEnd;
        foreach (var node in CurrentBoard)
        {
            node.Affiliation.Changed += OnNodeAffiliationChanged;
        }

        _buttonController.Init(CurrentBoard);
    }

    protected void DoStartGame()
    {
        _playing = true;
        if (GameStart != null)
        {
            GameStart();
        }
    }

    protected void InitializeBoardInfoClass(BoardData boardData)
    {
        if (string.IsNullOrEmpty(boardData.InfoClassName)) return;
        try
        {
            var type = Type.GetType(boardData.InfoClassName);
            var obj = gameObject.AddComponent(type);
            if (!(obj is BaseBoardInfo))
            {
                Debug.LogWarning("Info script does not inherit BaseBoardInfo.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public bool AdvanceToNextLevel()
    {
        if (HasNextLevel)
        {
            _startingBoardIndex++;
        }
        else if (HasNextSeries)
        {
            _boardSeriesIndex++;
            _startingBoardIndex = 0;
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

    private void Awake()
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

    protected void Start()
    {
        // UI Events
        EnergyPoolManager = new EnergyPoolManager(_energyFactory);
        _buttonController.NodeSwipeOccurred += OnNodeSwipeOccurred;
        _buttonController.SwipeOccurred += OnSwipeOccurred;
    }

    private void Update()
    {
        if (_playing)
        {
            GameTime += Time.deltaTime;
        }
    }

    void OnDestory()
    {
        CurrentBoard.Behavior.SpinEnd -= OnSpinEnd;
    }

    private void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
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
        _playing = false;
        if (GameEnd != null)
        {
            GameEnd();
        }
    }

    private void OnNodeSwipeOccurred(NodeButtonBehavior down, NodeButtonBehavior up, Vector2 dir)
    {
        var fromNode = CurrentBoard.GetOffsetNode(down.XIndex, down.YIndex);
        var toNode = CurrentBoard.GetOffsetNode(up.XIndex, up.YIndex);

        if (fromNode == null || toNode == null) return;
        var dist = Vector2.Distance(fromNode.Behavior.transform.position, toNode.Behavior.transform.position);
        if (!(dist <= 1.25f)) return;
        fromNode.SendEnergy(toNode);
        EnergyTransferCount++;
    }

    private void OnSwipeOccurred(Vector2 dir)
    {
        CurrentBoard.Behavior.Spin(dir);
        if (_downButton == null) return;
        _downButton.Deselect();
        _downButton = null;
    }

    private void OnSpinEnd()
    {
        BoardTurnCount++;
    }
}

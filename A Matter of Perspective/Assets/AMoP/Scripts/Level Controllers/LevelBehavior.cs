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
    
    private BoardNode selectedNode;
    private NodeButtonBehavior downButton;
    private bool playing = false;

    public Board CurrentBoard { get; private set; }
    public EnergyPoolManager EnergyPoolManager { get; private set; }
    public float GameTime { get; private set; }
    public bool HasNextLevel
    {
        get
        {
            var boardSeries = GameData.SeriesList.GetSeries(boardSeriesIndex);
            return startingBoardIndex+1 < boardSeries.Count;
        }
    }
    public bool HasNextSeries { get { return boardSeriesIndex + 1 < GameData.SeriesList.Count; } }

    public void StartGame(int seriesIndex, int boardIndex)
    {
        boardSeriesIndex = seriesIndex;
        startingBoardIndex = boardIndex;
        StartGame();
    }

    public void StartGame()
    {
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
        CurrentBoard = new Board(boardData, boardBehavior, boardNodeFactory);
        CurrentBoard.Behavior.SpinEnd += OnSpinEnd;
        foreach (var node in CurrentBoard)
        {
            node.Affiliation.Changed += OnNodeAffiliationChanged;
        }

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
        buttonController.NodeButtonPointerDown += OnNodeButtonDown;
        buttonController.NodeButtonPointerUp += OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter += OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit += OnNodeButtonExit;
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
        buttonController.NodeButtonPointerDown -= OnNodeButtonDown;
        buttonController.NodeButtonPointerUp -= OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter -= OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit -= OnNodeButtonExit;
        buttonController.SwipeOccurred -= OnSwipeOccurred;
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

    void OnNodeButtonDown(NodeButtonBehavior button)
    {
        BoardNode node = CurrentBoard.GetNode(button.XIndex, button.YIndex);
        if (node != null && node.Affiliation.Value == BoardNodeAffiliation.Player && node.CanSend)
        {
            button.Select();
            selectedNode = node;
            downButton = button;
        }
    }

    void OnNodeButtonUp(NodeButtonBehavior button)
    {
        if (selectedNode != null)
        {
            BoardNode node = CurrentBoard.GetNode(button.XIndex, button.YIndex);
            // Hide selected node
            downButton.Deselect();
            // Keep hovered node highlighted after pointer up
            if (selectedNode == node)
            {
                downButton.Hover();
            }
            else if (node != null)
            {
                // Figure direction of swipe
                var sNodePos = selectedNode.Behavior.transform.position;
                var nodePos = node.Behavior.transform.position;
                Vector2 dir = MathUtils.ClosestCardinal(nodePos - sNodePos);
                // Find node in that direction of selectedNode
                float nodeX = selectedNode.Behavior.transform.position.x + 2.5f + dir.x; 
                float nodeY = selectedNode.Behavior.transform.position.y + 2.5f + dir.y;

                BoardNode adjacentNode = CurrentBoard.GetNode(Mathf.RoundToInt(nodeX), Mathf.RoundToInt(nodeY));
                if (adjacentNode != null && adjacentNode != selectedNode)
                {
                    // Perform energy transfer
                    selectedNode.SendEnergy(adjacentNode);
                }
            }
            downButton = null;
            selectedNode = null;
        }
        else if (downButton != null)
        {
            Vector2 dir = new Vector2(button.XIndex - downButton.XIndex, button.YIndex - downButton.YIndex).normalized;
            CurrentBoard.Behavior.Spin(MathUtils.ClosestCardinal(dir));
            downButton.Deselect();
            downButton = null;
            selectedNode = null;
        }
    }

    void OnNodeButtonEnter(NodeButtonBehavior button)
    {
        BoardNode node = CurrentBoard.GetNode(button.XIndex, button.YIndex);

        // Nothing selected or is the same as previously selected
        if (node == null || node == selectedNode)
        {
            return;
        }

        // If nothing previously selected 
        if (selectedNode == null)
        {
            // Player owned node and is player node and node is flagged to send
            if (node.Affiliation.Value == BoardNodeAffiliation.Player && node.CanSend)
            {
                button.Hover();
            }
        }
        else // Node previously selected
        { 
            // adjacent node
            if (Vector2.Distance(selectedNode.PerspectivePos, node.PerspectivePos) <= 1.25f && node.CanReceive)
            {
                button.Hover();
            }
        }
    }

    void OnNodeButtonExit(NodeButtonBehavior button)
    {
        BoardNode node = CurrentBoard.GetNode(button.XIndex, button.YIndex);
        if (node != null && node != selectedNode)
        {
            button.Unhover();
        }
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        CurrentBoard.Behavior.Spin(dir);
        if (downButton != null)
        {
            downButton.Deselect();
            downButton = null;
        }
        selectedNode = null;
    }

    void OnSpinEnd()
    {
        // Hide all node buttons
    }
}

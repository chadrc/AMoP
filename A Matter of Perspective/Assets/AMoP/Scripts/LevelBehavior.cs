using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    public static LevelBehavior Current { get; private set; }

    [SerializeField]
    private BoardBehavior boardBehavior;

    [SerializeField]
    private BoardData boardData;

    [SerializeField]
    private BoardNodeFactory boardNodeFactory;

    [SerializeField]
    private EnergyFactory energyFactory;

    [SerializeField]
    private NodeButtonPanelViewController buttonController;

    private Board board;
    private BoardNode selectedNode;
    public EnergyPoolManager EnergyPoolManager { get; private set; }

    private NodeButtonBehavior downButton;

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

	// Use this for initialization
	void Start ()
    {
        EnergyPoolManager = new EnergyPoolManager(energyFactory);
        board = new Board(boardData, boardBehavior, boardNodeFactory);
        board.Behavior.SpinEnd += OnSpinEnd;
        buttonController.NodeButtonPointerDown += OnNodeButtonDown;
        buttonController.NodeButtonPointerUp += OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter += OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit += OnNodeButtonExit;
        buttonController.SwipeOccurred += OnSwipeOccurred;
	}

    void OnDestroy()
    {
        buttonController.NodeButtonPointerDown -= OnNodeButtonDown;
        buttonController.NodeButtonPointerUp -= OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter -= OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit -= OnNodeButtonExit;
        buttonController.SwipeOccurred -= OnSwipeOccurred;
        board.Behavior.SpinEnd -= OnSpinEnd;
    }

    void OnNodeButtonDown(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && node.Affiliation.Value == BoardNodeAffiliation.Player)
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
            BoardNode node = board.GetNode(button.XIndex, button.YIndex);
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
                BoardNode adjacentNode = board.GetNode(
                    (int)(selectedNode.Behavior.transform.position.x + 2.5f + dir.x), 
                    (int)(selectedNode.Behavior.transform.position.y + 2.5f + dir.y));
                if (adjacentNode != null)
                {
                    // Perform energy transfer
                    selectedNode.SendEnergy(adjacentNode);
                }
            }
            selectedNode = null;
        }
        else if (downButton != null)
        {
            Vector2 dir = new Vector2(button.XIndex - downButton.XIndex, button.YIndex - downButton.YIndex).normalized;
            board.Behavior.Spin(MathUtils.ClosestCardinal(dir));
            downButton.Deselect();
            downButton = null;
        }
    }

    void OnNodeButtonEnter(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && (selectedNode != null || node.Affiliation.Value == BoardNodeAffiliation.Player) && node != selectedNode)
        {
            button.Hover();
        }
    }

    void OnNodeButtonExit(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && node != selectedNode)
        {
            button.Unhover();
        }
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        board.Behavior.Spin(dir);
    }

    void OnSpinEnd()
    {
        foreach (var node in board)
        {
            // Hide all buttons
        }
    }
}

using UnityEngine;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    [SerializeField]
    private BoardBehavior boardBehavior;

    [SerializeField]
    private BoardData boardData;

    [SerializeField]
    private BoardNodeFactory boardNodeFactory;

    [SerializeField]
    private NodeButtonPanelViewController buttonController;

    private Board board;
    private bool canSwipe = true;
    private BoardNode selectedNode;

    private NodeButtonBehavior downButton;

	// Use this for initialization
	void Start ()
    {
        board = new Board(boardData, boardBehavior, boardNodeFactory);
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
    }

    void OnNodeButtonDown(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null)
        {
            node.Behavior.Select();
            selectedNode = node;
        }
        else
        {
            downButton = button;
        }
    }

    void OnNodeButtonUp(NodeButtonBehavior button)
    {
        if (selectedNode != null)
        {
            BoardNode node = board.GetNode(button.XIndex, button.YIndex);
            selectedNode.Behavior.Deselect();
            if (selectedNode == node)
            {
                selectedNode.Behavior.Highlight();
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
                    BoardNode.TransferEnergy(selectedNode, adjacentNode);
                }
            }
            selectedNode = null;
        }
        else if (downButton != null)
        {
            Vector2 dir = new Vector2(button.XIndex - downButton.XIndex, button.YIndex - downButton.YIndex).normalized;
            board.Behavior.Spin(MathUtils.ClosestCardinal(dir));
            downButton = null;
        }
    }

    void OnNodeButtonEnter(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && node != selectedNode)
        {
            node.Behavior.Highlight();
        }
    }

    void OnNodeButtonExit(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && node != selectedNode)
        {
            node.Behavior.Unhighlight();
        }
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        board.Behavior.Spin(dir);
    }
}

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
        buttonController.SwipeOccurred += OnSwipeOccurred;
	}

    void OnDestroy()
    {
        buttonController.NodeButtonPointerDown -= OnNodeButtonDown;
        buttonController.NodeButtonPointerUp -= OnNodeButtonUp;
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
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (selectedNode != null)
        {
            selectedNode.Behavior.Deselect();
            selectedNode = null;
        }
        else if (downButton != null)
        {
            Vector2 dir = new Vector2(button.XIndex - downButton.XIndex, button.YIndex - downButton.YIndex).normalized;
            board.Behavior.Spin(MathUtils.ClosestCardinal(dir));
            downButton = null;
        }
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        board.Behavior.Spin(dir);
    }
}

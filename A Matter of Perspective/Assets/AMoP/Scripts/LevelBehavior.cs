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
    }

    void OnNodeButtonUp(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (selectedNode != null)
        {
            selectedNode.Behavior.Deselect();
        }
    }

    void OnSwipeOccurred(Vector2 dir)
    {
        if (canSwipe)
        {
            StartCoroutine(boardSpin(dir));
        }
    }

    IEnumerator boardSpin(Vector2 dir)
    {
        canSwipe = false;
        float spinTime = .25f;
        float t = 0;
        Quaternion startRot = board.Behavior.transform.rotation;
        board.Behavior.transform.Rotate(Vector3.up, 90f * -dir.x, Space.World);
        board.Behavior.transform.Rotate(Vector3.right, 90f * dir.y, Space.World);
        Quaternion endRot = board.Behavior.transform.rotation;
        board.Behavior.transform.rotation = startRot;

        while (t < spinTime)
        {
            t += Time.deltaTime;
            float frac = Mathf.Clamp01(t / spinTime);
            board.Behavior.transform.rotation = Quaternion.Slerp(startRot, endRot, frac);

            yield return new WaitForEndOfFrame();
        }

        canSwipe = true;
    }
}

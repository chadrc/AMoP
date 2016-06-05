using UnityEngine;
using UnityEngine.EventSystems;

public class LevelBehavior : MonoBehaviour
{
    [SerializeField]
    private BoardData boardData;

    [SerializeField]
    private BoardNodeFactory boardNodeFactory;

    [SerializeField]
    private NodeButtonPanelViewController buttonController;

	// Use this for initialization
	void Start () {
        Board board = new Board(boardData, boardNodeFactory);
        buttonController.NodeButtonPointerDown += OnNodeButtonDown;
        buttonController.NodeButtonPointerUp += OnNodeButtonUp;
	}

    void OnNodeButtonDown(NodeButtonBehavior button)
    {
        Debug.Log(button.XIndex + ", " + button.YIndex);
    }

    void OnNodeButtonUp(NodeButtonBehavior button)
    {
        Debug.Log(button.XIndex + ", " + button.YIndex);
    }
}

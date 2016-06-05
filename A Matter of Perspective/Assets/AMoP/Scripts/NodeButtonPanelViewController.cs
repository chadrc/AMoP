using UnityEngine;
using System.Collections.Generic;

public class NodeButtonPanelViewController : MonoBehaviour
{
    [SerializeField]
    private GameObject nodeButtonPrefab;
    
    // Sends x and y index of button pressed
    public event System.Action<NodeButtonBehavior> NodeButtonPointerDown;
    // Sends direction from original press in closest cardinal
    public event System.Action<NodeButtonBehavior> NodeButtonPointerUp;

    private List<NodeButtonBehavior> nodeButtons = new List<NodeButtonBehavior>();
    private NodeButtonBehavior lastEnter;

    // TODO: Refactor out resize checking into separate behavior with event 
    private int InitialScreenWidth;
    private int InitialScreenHeight;

	// Use this for initialization
	void Start ()
    {
        initialize();
    }

    void Update()
    {
        if (Screen.width != InitialScreenWidth ||
            Screen.height != InitialScreenHeight)
        {
            foreach(var button in nodeButtons)
            {
                GameObject.Destroy(button.gameObject);
            }
            nodeButtons.Clear();
            initialize();
        }
    }

    public void ButtonDown(NodeButtonBehavior button)
    {
        if (NodeButtonPointerDown != null)
        {
            NodeButtonPointerDown(button);
        }
        lastEnter = button;
    }

    public void ButtonEnter(NodeButtonBehavior button)
    {
        lastEnter = button;
    }

    public void ButtonUp(NodeButtonBehavior button)
    {
        if (lastEnter != null && NodeButtonPointerUp != null)
        {
            NodeButtonPointerUp(lastEnter);
        }
        lastEnter = null;
    }

    private void initialize()
    {
        InitialScreenHeight = Screen.height;
        InitialScreenWidth = Screen.width;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                createButton(i, j);
            }
        }
    }

    private void createButton(int x, int y)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(x - 2.5f, y - 2.5f, 0));
        var btnObj = GameObject.Instantiate(nodeButtonPrefab) as GameObject;
        var behavior = btnObj.GetComponent<NodeButtonBehavior>();

        btnObj.transform.SetParent(transform);
        btnObj.transform.localScale = Vector3.one;
        var rectTransform = btnObj.transform as RectTransform;
        float heightRatio = 1.0f / (Camera.main.orthographicSize * 2);
        float pixelHeight = Screen.height * heightRatio;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelHeight);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, pixelHeight);
        rectTransform.anchoredPosition = pos;

        behavior.Init(this, x, y);
        nodeButtons.Add(behavior);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class NodeButtonPanelViewController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject nodeButtonPrefab;
    
    public event Action<NodeButtonBehavior> NodeButtonPointerDown;
    public event Action<NodeButtonBehavior> NodeButtonPointerUp;
    public event Action<NodeButtonBehavior> NodeButtonPointerEnter;
    public event Action<NodeButtonBehavior> NodeButtonPointerExit;

    // Sends cardinal direction of swipe;
    public event Action<Vector2> SwipeOccurred;

    private List<NodeButtonBehavior> nodeButtons = new List<NodeButtonBehavior>();
    private NodeButtonBehavior lastEnter;

    // TODO: Refactor out resize checking into separate behavior with event 
    private int InitialScreenWidth;
    private int InitialScreenHeight;

    // Swipe Calculation Variables
    private Vector2 pointDown;

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

    public void ButtonUp(NodeButtonBehavior button)
    {
        if (lastEnter != null && NodeButtonPointerUp != null)
        {
            NodeButtonPointerUp(lastEnter);
        }
        lastEnter = null;
    }

    public void ButtonEnter(NodeButtonBehavior button)
    {
        lastEnter = button;
        if (NodeButtonPointerEnter != null)
        {
            NodeButtonPointerEnter(button);
        }
    }

    public void ButtonExit(NodeButtonBehavior button)
    {
        if (lastEnter == button)
        {
            lastEnter = null;
        }

        if (NodeButtonPointerExit != null)
        {
            NodeButtonPointerExit(button);
        }
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

    public void OnPointerDown(PointerEventData eventData)
    {
        pointDown = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 dif = eventData.position - pointDown;
        if (dif.magnitude < 1.0f)
        {
            // Ignore if didn't swipe far enough
            return;
        }

        Vector2 cardinal = MathUtils.ClosestCardinal(dif);

        if (SwipeOccurred != null)
        {
            SwipeOccurred(cardinal);
        }
    }
}

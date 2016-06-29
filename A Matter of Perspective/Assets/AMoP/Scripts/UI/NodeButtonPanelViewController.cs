using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class NodeButtonPanelViewController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject nodeButtonPrefab;

    // (Down button, Up button, direction)
    public event Action<NodeButtonBehavior, NodeButtonBehavior, Vector2> NodeSwipeOccurred;

    // Sends cardinal direction of swipe;
    public event Action<Vector2> SwipeOccurred;

    private Board board;
    private List<NodeButtonBehavior> nodeButtons = new List<NodeButtonBehavior>();
    private NodeButtonBehavior lastEnter;
    private NodeButtonBehavior downButton;
    
    // Swipe Calculation Variables
    private Vector2 pointDown;

    public void Init(Board board)
    {
        this.board = board;
    }

    // Use this for initialization
    void Awake ()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        StartCoroutine(initialize());
    }

    private void onScreenChanged(int width, int height)
    {
        foreach (var button in nodeButtons)
        {
            GameObject.Destroy(button.gameObject);
        }
        nodeButtons.Clear();
        StartCoroutine(initialize());
    }

    private IEnumerator initialize()
    {
        // Need to wait for camera to update fully before recreating buttons
        yield return new WaitForEndOfFrame();

        float ratio = Screen.width / (float)Screen.height;

        if (ratio >= 1.0f)
        {
            // Landscape
            Camera.main.orthographicSize = 4.0f;
        }
        else
        {
            // Portrait
            Camera.main.orthographicSize = 4.0f / ratio;
        }

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
        var btnObj = GameObject.Instantiate(nodeButtonPrefab) as GameObject;
        var behavior = btnObj.GetComponent<NodeButtonBehavior>();
        btnObj.transform.SetParent(transform);
        btnObj.transform.localScale = Vector3.one;
        Vector3 pos = Camera.main.WorldToScreenPoint(new Vector3(x - 2.5f, y - 2.5f, 0));
        (btnObj.transform as RectTransform).anchoredPosition = pos;
        behavior.Init(this, x, y);
        nodeButtons.Add(behavior);
    }

    public void ButtonEnter(NodeButtonBehavior button, PointerEventData data)
    {
        lastEnter = button;
        var node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && button != downButton)
        {
            if (downButton != null && node.CanReceive)
            {
                lastEnter.Hover();
            }
            else if (node.Affiliation == BoardNodeAffiliation.Player && node.CanSend)
            {
                lastEnter.Hover();
            }
        }
    }

    public void ButtonExit(NodeButtonBehavior button, PointerEventData data)
    {
        if (lastEnter != null && lastEnter != downButton)
        {
            lastEnter.Unhover();
        }

        if (lastEnter == button)
        {
            lastEnter = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointDown = eventData.position;
        if (lastEnter == null)
        {
            return;
        }

        var node = board.GetNode(lastEnter.XIndex, lastEnter.YIndex);
        if (node != null && node.Affiliation == BoardNodeAffiliation.Player && node.CanSend)
        {
            downButton = lastEnter;
            downButton.Select();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (downButton != null)
        {
            if (lastEnter != null)
            {
                lastEnter.Unhover();

                if (downButton != lastEnter)
                {
                    var node = board.GetNode(lastEnter.XIndex, lastEnter.YIndex);
                    if (node != null)
                    {
                        if (NodeSwipeOccurred != null)
                        {
                            NodeSwipeOccurred(downButton, lastEnter, MathUtils.ClosestCardinal(eventData.position - pointDown));
                        }
                    }
                    else
                    {
                        //RaiseSwipeOccurred(eventData);
                    }
                    downButton.Deselect();
                }
                else
                {
                    downButton.Hover();
                    lastEnter = downButton;
                    //RaiseSwipeOccurred(eventData);
                }
            }
            else
            {
                downButton.Deselect();
                //RaiseSwipeOccurred(eventData);
            }
        }
        else
        {
            RaiseSwipeOccurred(eventData);
        }
        
        downButton = null;
    }

    private void RaiseSwipeOccurred(PointerEventData eventData)
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

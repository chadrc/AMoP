﻿using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

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
    
    // Swipe Calculation Variables
    private Vector2 pointDown;

    // Use this for initialization
    void Awake ()
    {
        ScreenChangeListeningBehavior.ScreenChanged += onScreenChanged;
        LevelBehavior.GameStart += onGameStart;
        LevelBehavior.GameEnd += onGameEnd;
    }
    
    void OnDestory()
    {
        LevelBehavior.GameStart -= onGameStart;
        LevelBehavior.GameEnd -= onGameEnd;
    }

    public void ButtonDown(NodeButtonBehavior button, PointerEventData eventData)
    {
        if (NodeButtonPointerDown != null)
        {
            NodeButtonPointerDown(button);
        }
        pointDown = eventData.position;
        lastEnter = button;
    }

    public void ButtonUp(NodeButtonBehavior button, PointerEventData eventData)
    {
        if (lastEnter != null && NodeButtonPointerUp != null)
        {
            NodeButtonPointerUp(lastEnter);
        }
        else
        {
            RaiseSwipeOccurred(eventData);
        }
        lastEnter = null;
    }

    public void ButtonEnter(NodeButtonBehavior button, PointerEventData eventData)
    {
        lastEnter = button;
        if (NodeButtonPointerEnter != null)
        {
            NodeButtonPointerEnter(button);
        }
    }

    public void ButtonExit(NodeButtonBehavior button, PointerEventData eventData)
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

    private void onScreenChanged(int width, int height)
    {
        foreach (var button in nodeButtons)
        {
            GameObject.Destroy(button.gameObject);
        }
        nodeButtons.Clear();
        StartCoroutine(initialize());
    }

    private void onGameStart()
    {
        StartCoroutine(initialize());
    }

    private void onGameEnd()
    {
        foreach (var button in nodeButtons)
        {
            GameObject.Destroy(button.gameObject);
        }
        nodeButtons.Clear();
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

    public void OnPointerDown(PointerEventData eventData)
    {
        pointDown = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RaiseSwipeOccurred(eventData);
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

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class NodeButtonBehavior : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public NodeButtonPanelViewController Controller { get; private set; }
    public int XIndex { get; private set; }
    public int YIndex { get; private set; }

    public void Init(NodeButtonPanelViewController controller, int xIndex, int yIndex)
    {
        Controller = controller;
        XIndex = xIndex;
        YIndex = yIndex;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Controller.ButtonDown(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Controller.ButtonEnter(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Controller.ButtonUp(this);
    }
}

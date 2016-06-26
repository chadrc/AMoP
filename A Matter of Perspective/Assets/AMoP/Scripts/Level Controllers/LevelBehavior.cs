﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class LevelBehavior : MonoBehaviour
{
    public static LevelBehavior Current { get; private set; }
    public static event System.Action GameStart;
    public static event System.Action GameEnd;

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
    private NodeButtonBehavior downButton;
    private bool playing = false;

    public EnergyPoolManager EnergyPoolManager { get; private set; }
    public float GameTime { get; private set; }

    public void InitBoard()
    {
        destroyBoard();
        GameTime = 0;
        EnergyPoolManager = new EnergyPoolManager(energyFactory);
        board = new Board(boardData, boardBehavior, boardNodeFactory);
        board.Behavior.SpinEnd += OnSpinEnd;
        foreach (var node in board)
        {
            node.Affiliation.Changed += OnNodeAffiliationChanged;
        }

        // UI Events
        buttonController.NodeButtonPointerDown += OnNodeButtonDown;
        buttonController.NodeButtonPointerUp += OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter += OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit += OnNodeButtonExit;
        buttonController.SwipeOccurred += OnSwipeOccurred;

        playing = true;
        if (GameStart != null)
        {
            GameStart();
        }
    }

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

    void Update()
    {
        if (playing)
        {
            GameTime += Time.deltaTime;
        }
    }

    void OnDestory()
    {
        breakDownGame();
    }

    void breakDownGame()
    {
        buttonController.NodeButtonPointerDown -= OnNodeButtonDown;
        buttonController.NodeButtonPointerUp -= OnNodeButtonUp;
        buttonController.NodeButtonPointerEnter -= OnNodeButtonEnter;
        buttonController.NodeButtonPointerExit -= OnNodeButtonExit;
        buttonController.SwipeOccurred -= OnSwipeOccurred;
        board.Behavior.SpinEnd -= OnSpinEnd;
        foreach (var node in board)
        {
            node.Affiliation.Changed -= OnNodeAffiliationChanged;
        }
    }

    void destroyBoard()
    {
        if (board == null)
        {
            return;
        }

        foreach (var node in board)
        {
            GameObject.Destroy(node.Behavior.gameObject);
        }
        board.Behavior.Uninit();
        board = null;

        EnergyPoolManager.HideAllEnergy();
    }

    void OnNodeAffiliationChanged(BoardNodeAffiliation affiliation)
    {
        foreach (var node in board)
        {
            // If any node doesn't equal new affiliation, board still needs to be filled
            if (node.CanReceive && node.Affiliation != affiliation)
            {
                return;
            }
        }

        // Clean up resources
        playing = false;
        if (GameEnd != null)
        {
            GameEnd();
        }
    }

    void OnNodeButtonDown(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);
        if (node != null && node.Affiliation.Value == BoardNodeAffiliation.Player && node.CanSend)
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
                float nodeX = selectedNode.Behavior.transform.position.x + 2.5f + dir.x; 
                float nodeY = selectedNode.Behavior.transform.position.y + 2.5f + dir.y;

                BoardNode adjacentNode = board.GetNode(Mathf.RoundToInt(nodeX), Mathf.RoundToInt(nodeY));
                if (adjacentNode != null && adjacentNode != selectedNode)
                {
                    // Perform energy transfer
                    selectedNode.SendEnergy(adjacentNode);
                }
            }
            downButton = null;
            selectedNode = null;
        }
        else if (downButton != null)
        {
            Vector2 dir = new Vector2(button.XIndex - downButton.XIndex, button.YIndex - downButton.YIndex).normalized;
            board.Behavior.Spin(MathUtils.ClosestCardinal(dir));
            downButton.Deselect();
            downButton = null;
            selectedNode = null;
        }
    }

    void OnNodeButtonEnter(NodeButtonBehavior button)
    {
        BoardNode node = board.GetNode(button.XIndex, button.YIndex);

        // Nothing selected or is the same as previously selected
        if (node == null || node == selectedNode)
        {
            return;
        }

        // If nothing previously selected 
        if (selectedNode == null)
        {
            // Player owned node and is player node and node is flagged to send
            if (node.Affiliation.Value == BoardNodeAffiliation.Player && node.CanSend)
            {
                button.Hover();
            }
        }
        else // Node previously selected
        { 
            // adjacent node
            if (Vector2.Distance(selectedNode.PerspectivePos, node.PerspectivePos) <= 1.25f && node.CanReceive)
            {
                button.Hover();
            }
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
        if (downButton != null)
        {
            downButton.Deselect();
            downButton = null;
        }
        selectedNode = null;
    }

    void OnSpinEnd()
    {
        // Hide all node buttons
    }
}

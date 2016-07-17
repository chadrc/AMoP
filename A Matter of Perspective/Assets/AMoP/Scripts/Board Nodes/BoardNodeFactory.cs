using UnityEngine;
using System.Collections.Generic;

public class BoardNodeFactory : ScriptableObject
{
    [SerializeField]
    private GameObject BasicBoardNodePrefab;

    [SerializeField]
    private GameObject DrainBoardNodePrefab;

    [SerializeField]
    private GameObject FillBoardNodePrefab;

    [SerializeField]
    private GameObject NullBoardNodePrefab;

    [SerializeField]
    private GameObject PoolBoardNodePrefab;

    [SerializeField]
    private GameObject VortexBoardNodePrefab;

    [SerializeField]
    private GameObject RedirectBoardNodePrefab;

    public BoardNode CreateNode(BoardNodeData data, Board parent)
    {
        GameObject prefab = null;
        BoardNode node = null;
        #pragma warning disable 0162 // Unreachable code
        switch (data.Type)
        {
            case BoardNodeType.Basic:
                prefab = BasicBoardNodePrefab;
                node = new BasicBoardNode(data, parent);
                break;

            case BoardNodeType.Drain:
                prefab = DrainBoardNodePrefab;
                node = new DrainBoardNode(data, parent);
                break;

            case BoardNodeType.Fill:
                prefab = FillBoardNodePrefab;
                node = new FillBoardNode(data, parent);
                break;

            case BoardNodeType.Null:
                prefab = NullBoardNodePrefab;
                node = new NullBoardNode(data, parent);
                break;

            case BoardNodeType.Pool:
                prefab = PoolBoardNodePrefab;
                node = new PoolBoardNode(data, parent);
                break;

            case BoardNodeType.Redirect:
                prefab = RedirectBoardNodePrefab;
                node = new RedirectBoardNode(data, parent);
                break;

            case BoardNodeType.Vortex:
                prefab = VortexBoardNodePrefab;
                node = new VortexBoardNode(data, parent);
                break;
        }
        #pragma warning restore 0162

        var nodeObj = GameObject.Instantiate(prefab) as GameObject;
        var behavior = nodeObj.GetComponent<BoardNodeBehavior>();

        node.AttachedToBehavior(behavior);
        behavior.AttachToNode(node);

        return node;
    }
}
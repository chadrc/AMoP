using UnityEngine;
using System.Collections.Generic;

public class BoardNodeFactory : ScriptableObject
{
    [SerializeField]
    private GameObject BasicBoardNodePrefab;

    [SerializeField]
    private GameObject PoolBoardNodePrefab;

    [SerializeField]
    private GameObject VortexBoardNodePrefab;

    [SerializeField]
    private GameObject BlockBoardNodePrefab;

    [SerializeField]
    private GameObject MovingBoardNodePrefab;

    [SerializeField]
    private GameObject NullBoardNodePrefab;

    public BoardNode CreateNode(BoardNodeData data)
    {
        GameObject prefab = null;
        BoardNode node = null;

        switch (data.Type)
        {
            case BoardNodeType.Basic:
                prefab = BasicBoardNodePrefab;
                node = new BasicBoardNode(data);
                break;

            case BoardNodeType.Block:
                prefab = BlockBoardNodePrefab;
                throw new System.NotImplementedException("Block board node not implemented.");
                break;

            case BoardNodeType.Moving:
                prefab = MovingBoardNodePrefab;
                throw new System.NotImplementedException("Moving board node not implemented.");
                break;

            case BoardNodeType.Null:
                prefab = NullBoardNodePrefab;
                node = new NullBoardNode(data);
                break;

            case BoardNodeType.Pool:
                prefab = PoolBoardNodePrefab;
                node = new PoolBoardNode(data);
                break;

            case BoardNodeType.Vortex:
                prefab = VortexBoardNodePrefab;
                throw new System.NotImplementedException("Vortex board node not implemented.");
                break;
        }

        var nodeObj = GameObject.Instantiate(prefab) as GameObject;
        var behavior = nodeObj.GetComponent<BoardNodeBehavior>();

        node.AttachedToBehavior(behavior);
        behavior.AttachToNode(node);

        return node;
    }
}
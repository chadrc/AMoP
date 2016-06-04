using UnityEngine;
using System.Collections;

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
        var node = new BoardNode(data);

        var nodeObj = GameObject.Instantiate(getPrefabForType(data.Type)) as GameObject;
        var behavior = nodeObj.GetComponent<BoardNodeBehavior>();

        node.AttachedToBehavior(behavior);
        behavior.AttachToNode(node);

        return node;
    }

    private GameObject getPrefabForType(BoardNodeType type)
    {
        GameObject prefab = null;

        switch(type)
        {
            case BoardNodeType.Basic:
                prefab = BasicBoardNodePrefab;
                break;

            case BoardNodeType.Block:
                prefab = BlockBoardNodePrefab;
                break;
                
            case BoardNodeType.Moving:
                prefab = MovingBoardNodePrefab;
                break;

            case BoardNodeType.Null:
                prefab = NullBoardNodePrefab;
                break;

            case BoardNodeType.Pool:
                prefab = PoolBoardNodePrefab;
                break;

            case BoardNodeType.Vortex:
                prefab = VortexBoardNodePrefab;
                break;
        }

        return prefab;
    }
}
using UnityEngine;
using System.Collections;

public class EditorBoardNodeBehavior : MonoBehaviour
{
    public BoardNodeData data;

    void Awake()
    {
        // Should not exist in Play mode, only for editor
        GameObject.Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class EditorBoardNodeBehavior : MonoBehaviour
{
    public BoardNodeData data;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .75f);
    }
}

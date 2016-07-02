using UnityEngine;
using System.Collections;

public class EditorBoardNodeBehavior : MonoBehaviour
{
    public BoardNodeData data;

    void OnDrawGizmos()
    {
		var clr = Color.white;

		switch (data.Type) {
		case BoardNodeType.Basic:
			clr = Color.green;
			break;

		case BoardNodeType.Pool:
			clr = Color.blue;
			break;

		case BoardNodeType.Null:
			clr = Color.black;
			break;

		case BoardNodeType.Moving:
			clr = Color.red;
			break;

		case BoardNodeType.Block:
			clr = Color.grey;
			break;

		case BoardNodeType.Vortex:
			clr = Color.cyan;
			break;
		}

        Gizmos.color = clr;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}

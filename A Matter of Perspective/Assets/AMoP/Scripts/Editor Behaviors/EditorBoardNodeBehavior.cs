using UnityEngine;
using System.Collections.Generic;

public class EditorBoardNodeBehavior : MonoBehaviour
{
	public static Dictionary<BoardNodeType, Color> TypeColorMap = new Dictionary<BoardNodeType, Color> () 
	{
		{BoardNodeType.Basic, new Color(0, 1, 0)},
		{BoardNodeType.Pool, new Color(0, 0, 1)},
		{BoardNodeType.Null, new Color(0, 0, 0)},
		{BoardNodeType.Moving, new Color(0, 1, 1)},
		{BoardNodeType.Block, new Color(1, 1, 0)},
		{BoardNodeType.Vortex, new Color(.5f, 0, .5f)},
	};

    public BoardNodeData data;

    void OnDrawGizmos()
    {
		var clr = TypeColorMap [data.Type];
        Gizmos.color = clr;
        Gizmos.DrawSphere(transform.position, .5f);
    }
}

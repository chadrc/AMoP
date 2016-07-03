using UnityEngine;
using System.Collections.Generic;

public class EditorBoardNodeBehavior : MonoBehaviour
{
	public static System.Func<int, BoardNodeData> GetBoardNodeData;

	public static event System.Action<EditorBoardNodeBehavior, bool> Edited;

	public static Dictionary<BoardNodeType, Color> TypeColorMap = new Dictionary<BoardNodeType, Color> () 
	{
		{BoardNodeType.Basic, new Color(0, 1, 0)},
		{BoardNodeType.Pool, new Color(0, 0, 1)},
		{BoardNodeType.Null, new Color(0, 0, 0)},
		{BoardNodeType.Moving, new Color(0, 1, 1)},
		{BoardNodeType.Block, new Color(1, 1, 0)},
		{BoardNodeType.Vortex, new Color(.5f, 0, .5f)},
	};

	[SerializeField]
	private int nodeIndex;

	public BoardNodeData Data { get { return GetBoardNodeData == null ? null : GetBoardNodeData(NodeIndex); } }
	public int NodeIndex { get { return nodeIndex; } }

    void OnDrawGizmos()
	{
		if (Data != null)
		{
			var clr = TypeColorMap [Data.Type];
			Gizmos.color = clr;
			Gizmos.DrawSphere(transform.position, .5f);
		}
    }

	public void SetData(int index)
	{
		gameObject.name = "Board Node: " + index;
		nodeIndex = index;
		this.transform.position = Data.Position;
	}

	public void InspectorEdited(bool delete = false)
	{
		this.transform.position = Data.Position;
		if (Edited != null)
		{
			Edited (this, delete);
		}
	}
}

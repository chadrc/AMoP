using UnityEngine;
using System.Collections.Generic;

public class EditorBoardNodeBehavior : MonoBehaviour
{
	public static System.Func<int, BoardNodeData> GetBoardNodeData;

	public static event System.Action<EditorBoardNodeBehavior, bool> Edited;

	public static Dictionary<BoardNodeType, Color> TypeColorMap = new Dictionary<BoardNodeType, Color> () 
	{
		{BoardNodeType.Basic, new Color(0, 1, 0)},
        {BoardNodeType.Drain, new Color(1, .5f, 0)},
        {BoardNodeType.Fill, new Color(1, 1, 0)},
        {BoardNodeType.Null, new Color(0, 0, 0)},
        {BoardNodeType.Pool, new Color(0, 0, 1)},
		{BoardNodeType.Redirect, new Color(0, 1, 1)},
		{BoardNodeType.Vortex, new Color(.5f, 0, .5f)},
	};

    private static float? nodeMaxEnergy;
    private static float? NodeMaxEnergy
    {
        get
        {
            if (nodeMaxEnergy == null)
            {
                nodeMaxEnergy = GameObject.Find("GameData").GetComponent<GameData>().ConstantsData.NodeMaxEnergy;
            }

            return nodeMaxEnergy;
        }
    }

	[SerializeField]
	private int nodeIndex;

	[SerializeField]
	private bool hidden;

	[SerializeField]
	private float alpha = 1.0f;

	public BoardNodeData Data { get { return GetBoardNodeData == null ? null : GetBoardNodeData(NodeIndex); } }
	public int NodeIndex { get { return nodeIndex; } }

	private Vector3 truePos { get; set; }

	// Mimic min and max scale of basic board node
	private const float minSize = .5f * .25f;
	private const float maxSize = .5f * .75f;

    void OnDrawGizmos()
	{
		if (Data != null && !hidden)
		{
			var clr = TypeColorMap [Data.Type];
			clr.a = alpha;
			Gizmos.color = clr;
            if (NodeMaxEnergy != null)
            {
                float size = Mathf.Lerp(minSize, maxSize, Data.StartingEnergy / (float)NodeMaxEnergy);
                Gizmos.DrawSphere(transform.position, size);
            }
		}
    }

	public void SetData(int index, BoardData board)
	{
		gameObject.name = "Board Node: " + index;
		nodeIndex = index;
        if (Data == null)
        {
            return;
        }
        truePos = Data.Position - new Vector3(board.OffsetValue, board.OffsetValue, board.OffsetValue);
		transform.localPosition = truePos;
	}

	public void InspectorEdited(bool delete = false)
	{
		this.transform.localPosition = truePos;
		if (Edited != null)
		{
			Edited (this, delete);
		}
	}

	public void Show()
	{
		hidden = false;
		alpha = 1.0f;
	}

	public void Hide()
	{
		hidden = true;
	}

	public void Fade()
	{
		hidden = false;
		alpha = .25f;
	}
}

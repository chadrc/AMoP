﻿using UnityEngine;
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

	[SerializeField]
	private int nodeIndex;

	[SerializeField]
	private bool hidden;

	[SerializeField]
	private float alpha = 1.0f;

	public BoardNodeData Data { get { return GetBoardNodeData == null ? null : GetBoardNodeData(NodeIndex); } }
	public int NodeIndex { get { return nodeIndex; } }

	private Vector3 truePos { get { return Data == null ? Vector3.zero : Data.Position - new Vector3 (2.5f, 2.5f, 2.5f); } }

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
			float size = Mathf.Lerp (minSize, maxSize, Data.StartingEnergy / 20f);
			Gizmos.DrawSphere(transform.position, size);
		}
    }

	public void SetData(int index)
	{
		gameObject.name = "Board Node: " + index;
		nodeIndex = index;
		this.transform.localPosition = truePos;
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

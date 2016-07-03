using UnityEngine;
using System.Collections.Generic;

public static class AMoPUtils
{
    public static Color GetColorForAffiliation(BoardNodeAffiliation affiliation)
    {
        Color newClr;
        switch (affiliation)
        {
            case BoardNodeAffiliation.Player:
                newClr = Color.cyan;
                break;

            case BoardNodeAffiliation.Enemy:
                newClr = new Color(1.0f, 1.0f, 0);
                break;

            case BoardNodeAffiliation.Neutral:
                newClr = Color.white;
                break;

            default:
                newClr = Color.magenta;
                break;
        }
        return newClr;
    }

	public static NodeRow GetNodeRow(List<BoardNode> nodes, int x, int y)
	{
		List<MonoBehaviour> behaviorList = new List<MonoBehaviour> ();
		foreach (var n in nodes)
		{
			behaviorList.Add (n.Behavior);
		}

		var row = GetBehaviorRow (behaviorList, x, y);

		List<BoardNode> hidden = new List<BoardNode> ();

		foreach (var b in row.Hidden)
		{
			hidden.Add (((BoardNodeBehavior)b).Node);
		}

		BoardNode closest = null;
		try
		{
			closest = ((BoardNodeBehavior)row.Closest).Node;
		}
		catch (System.Exception) 
		{

		}

		return new NodeRow (closest, hidden);
	}

	public static EditNodeRow GetEditNodeRow(List<EditorBoardNodeBehavior> editNodes, int x, int y)
	{
		List<MonoBehaviour> behaviorList = new List<MonoBehaviour> ();
		foreach (var n in editNodes)
		{
			behaviorList.Add (n);
		}

		var row = GetBehaviorRow (behaviorList, x, y);

		var hidden = new List<EditorBoardNodeBehavior> ();

		foreach (var b in row.Hidden)
		{
			hidden.Add ((EditorBoardNodeBehavior)b);
		}

		EditorBoardNodeBehavior closest = null;
		try
		{
			closest = (EditorBoardNodeBehavior)row.Closest;
		}
		catch (System.Exception) 
		{

		}

		return new EditNodeRow (closest, hidden);
	}

	private static BehaviorRow GetBehaviorRow(List<MonoBehaviour> behaviors, int x, int y)
	{
		float posX = x - 2.5f;
		float posY = y - 2.5f;

		var xyNodes = new List<MonoBehaviour>();
		foreach (var node in behaviors)
		{
			// Check for position in 1 unit range to account for float errors
			if (node.transform.position.x > posX - .5f && node.transform.position.x < posX + .5f &&
				node.transform.position.y > posY - .5f && node.transform.position.y < posY + .5f)
			{
				xyNodes.Add(node);
			}
		}

		MonoBehaviour matchNode = null;

		foreach (var node in xyNodes)
		{
			if (matchNode == null || matchNode.transform.position.z > node.transform.position.z)
			{
				matchNode = node;
			}
		}

		if (matchNode != null)
		{
			xyNodes.Remove(matchNode);
		}

		return new BehaviorRow(matchNode, xyNodes);
	}

	private class BehaviorRow 
	{
		public MonoBehaviour Closest { get; private set; }
		public List<MonoBehaviour> Hidden { get; private set; }

		public BehaviorRow(MonoBehaviour closest, List<MonoBehaviour> hidden)
		{
			Closest = closest;
			Hidden = hidden;
		}
	}
}

public class EditNodeRow
{
	public EditorBoardNodeBehavior Closest { get; private set; }
	public List<EditorBoardNodeBehavior> Hidden { get; private set; }

	public EditNodeRow(EditorBoardNodeBehavior closest, List<EditorBoardNodeBehavior> hidden)
	{
		Closest = closest;
		Hidden = hidden;
	}
}
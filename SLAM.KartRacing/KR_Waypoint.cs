using System.Collections.Generic;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_Waypoint
{
	private Transform root;

	public string Name => root.name;

	public Transform Left { get; private set; }

	public Transform Center { get; private set; }

	public Transform Right { get; private set; }

	public List<KR_Waypoint> Neighbours { get; private set; }

	public KR_Path Path { get; private set; }

	public KR_Waypoint(Transform waypointRoot, KR_Path path)
	{
		Path = path;
		Neighbours = new List<KR_Waypoint>();
		for (int i = 0; i < waypointRoot.childCount; i++)
		{
			Transform child = waypointRoot.GetChild(i);
			string text = child.name.ToUpper();
			if (text.StartsWith("L"))
			{
				Left = child;
			}
			else if (text.StartsWith("R"))
			{
				Right = child;
			}
			else if (text.StartsWith("C"))
			{
				Center = child;
			}
			else
			{
				Debug.LogError("Error parsing waypoints, unknown object found: " + child.name, child);
			}
		}
		root = waypointRoot;
	}

	public Vector3 GetClosestPoint(Vector3 position)
	{
		return MathUtilities.GetClosetPoint(Left.position, Right.position, position, segmentClamp: true);
	}

	public override string ToString()
	{
		return Name;
	}
}

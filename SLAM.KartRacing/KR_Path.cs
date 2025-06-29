using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_Path
{
	public List<KR_Waypoint> Waypoints = new List<KR_Waypoint>();

	public float Length { get; private set; }

	public KR_Route Route { get; private set; }

	public KR_Path(Transform pathRoot, KR_Route route)
	{
		Route = route;
		float num = 0f;
		KR_Waypoint kR_Waypoint = null;
		for (int i = 0; i < pathRoot.childCount; i++)
		{
			KR_Waypoint kR_Waypoint2 = new KR_Waypoint(pathRoot.GetChild(i), this);
			Waypoints.Add(kR_Waypoint2);
			if (kR_Waypoint != null)
			{
				num += Vector3.Distance(kR_Waypoint.Center.position, kR_Waypoint2.Center.position);
			}
			kR_Waypoint = kR_Waypoint2;
		}
		Length = num;
	}

	public float DistanceFromBeginTo(KR_Waypoint endPoint)
	{
		float num = 0f;
		bool flag = false;
		for (int num2 = Waypoints.Count - 1; num2 >= 0; num2--)
		{
			if (flag)
			{
				num += Vector3.Distance(Waypoints[num2].Center.position, Waypoints[num2 + 1].Center.position);
			}
			if (Waypoints[num2] == endPoint)
			{
				flag = true;
			}
		}
		return num;
	}

	public KR_Waypoint GetNextWayPoint(KR_Waypoint waypoint)
	{
		if (Waypoints.Last() == waypoint)
		{
			return null;
		}
		return Waypoints.ElementAt(Waypoints.IndexOf(waypoint) + 1);
	}

	public KR_Waypoint GetPreviousWayPoint(KR_Waypoint waypoint)
	{
		if (Waypoints.First() == waypoint)
		{
			return null;
		}
		return Waypoints.ElementAt(Waypoints.IndexOf(waypoint) - 1);
	}
}

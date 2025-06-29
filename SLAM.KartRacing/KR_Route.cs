using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_Route
{
	public List<KR_Path> Paths = new List<KR_Path>();

	public KR_Track Track { get; private set; }

	public float DefaultLength { get; private set; }

	public KR_Route(List<Transform> pathTransforms, KR_Track track)
	{
		Track = track;
		for (int i = 0; i < pathTransforms.Count; i++)
		{
			KR_Path item = new KR_Path(pathTransforms[i], this);
			Paths.Add(item);
		}
		if (Paths.Count > 0)
		{
			DefaultLength = Paths[0].Length;
		}
	}

	public KR_Path GetNextPath(KR_Path path)
	{
		if (Track.Routes.Last() == path.Route)
		{
			return null;
		}
		return Track.GetNextRoute(path.Route).Paths.First();
	}

	public KR_Path GetPreviousPath(KR_Path path)
	{
		if (Track.Routes.First() == path.Route)
		{
			return null;
		}
		return Track.GetPreviousRoute(path.Route).Paths.First();
	}
}

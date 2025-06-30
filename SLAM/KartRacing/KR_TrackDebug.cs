using UnityEngine;

namespace SLAM.KartRacing;

public class KR_TrackDebug : MonoBehaviour
{
	private KR_Track track;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void onRaceStarted(KR_StartRaceEvent evt)
	{
		track = evt.Track;
	}

	private void OnDrawGizmos()
	{
		if (!Application.isPlaying || track == null)
		{
			return;
		}
		for (int i = 0; i < track.Routes.Count; i++)
		{
			KR_Route kR_Route = track.Routes[i];
			for (int j = 0; j < kR_Route.Paths.Count; j++)
			{
				KR_Path kR_Path = kR_Route.Paths[j];
				for (int k = 0; k < kR_Path.Waypoints.Count; k++)
				{
					KR_Waypoint kR_Waypoint = kR_Path.Waypoints[k];
					KR_Waypoint kR_Waypoint2 = null;
					if (k + 1 < kR_Path.Waypoints.Count)
					{
						kR_Waypoint2 = kR_Path.Waypoints[k + 1];
					}
					if (kR_Waypoint2 != null)
					{
						Gizmos.color = Color.red;
						Gizmos.DrawLine(kR_Waypoint.Center.position, kR_Waypoint2.Center.position);
						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(kR_Waypoint.Right.position, kR_Waypoint2.Right.position);
						Gizmos.color = Color.cyan;
						Gizmos.DrawLine(kR_Waypoint.Left.position, kR_Waypoint2.Left.position);
					}
					Gizmos.color = Color.blue;
					Gizmos.DrawLine(kR_Waypoint.Left.position, kR_Waypoint.Right.position);
					Gizmos.color = Color.yellow;
					Gizmos.DrawSphere(kR_Waypoint.Center.position, 0.3f);
				}
			}
		}
	}
}

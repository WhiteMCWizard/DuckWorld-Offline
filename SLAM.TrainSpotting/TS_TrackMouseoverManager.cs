using UnityEngine;

namespace SLAM.TrainSpotting;

public class TS_TrackMouseoverManager : MonoBehaviour
{
	[SerializeField]
	private TSTrackMouseover[] tracks;

	[SerializeField]
	private LayerMask hitMask;

	private void Update()
	{
		TSTrackMouseover trackUnderMouse = getTrackUnderMouse();
		for (int i = 0; i < tracks.Length; i++)
		{
			tracks[i].SetMouseover(tracks[i] == trackUnderMouse);
		}
	}

	private TSTrackMouseover getTrackUnderMouse()
	{
		if (Camera.main == null)
		{
			return null;
		}
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(ray.origin, ray.direction);
		if (Physics.Raycast(ray, out var hitInfo, float.PositiveInfinity, hitMask.value))
		{
			return hitInfo.collider.GetComponentInParent<TSTrackMouseover>();
		}
		return null;
	}
}

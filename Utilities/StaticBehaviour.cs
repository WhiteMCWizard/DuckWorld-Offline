using SLAM.CameraSystem;
using UnityEngine;

public class StaticBehaviour : CameraBehaviour
{
	[SerializeField]
	private Transform location;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (location != null)
		{
			position = location.position;
			rotation = location.rotation;
		}
		else
		{
			position = Vector3.zero;
			rotation = Quaternion.identity;
		}
	}

	public void SetLocation(Transform newLocation)
	{
		location = newLocation;
	}
}

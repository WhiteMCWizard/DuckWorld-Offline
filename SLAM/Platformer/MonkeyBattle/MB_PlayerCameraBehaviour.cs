using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_PlayerCameraBehaviour : CameraBehaviour
{
	[SerializeField]
	public Transform target;

	[SerializeField]
	private float radius;

	[SerializeField]
	private Vector3 rotationOffset = Vector3.one;

	[SerializeField]
	private Vector3 positionOffset = Vector3.one;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			float f = Mathf.Atan2(target.transform.position.x, target.transform.position.z);
			Vector3 vector = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
			position = vector * radius;
			position.y = positionOffset.y;
			rotation = Quaternion.LookRotation(-vector) * Quaternion.Euler(rotationOffset);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}
}

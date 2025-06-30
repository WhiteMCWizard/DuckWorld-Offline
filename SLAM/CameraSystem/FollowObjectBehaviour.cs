using UnityEngine;

namespace SLAM.CameraSystem;

public class FollowObjectBehaviour : TargetObjectBehaviour
{
	[SerializeField]
	private Vector2 offsetDistance;

	[SerializeField]
	private float rotationDamping = 1f;

	[SerializeField]
	private float positionDamping = 1f;

	[SerializeField]
	private Vector3 rotationOffset;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			position = Vector3.Lerp(base.transform.position, target.position - target.forward * offsetDistance.x + target.up * offsetDistance.y, Time.deltaTime * positionDamping);
			rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(target.forward + rotationOffset), Time.deltaTime * rotationDamping);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}

	public void WarpTo()
	{
		base.transform.parent.position = target.position - target.forward * offsetDistance.x + target.up * offsetDistance.y;
		base.transform.parent.rotation = Quaternion.LookRotation(target.forward + rotationOffset);
	}
}

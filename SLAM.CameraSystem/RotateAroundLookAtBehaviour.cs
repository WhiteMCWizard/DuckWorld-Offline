using UnityEngine;

namespace SLAM.CameraSystem;

public class RotateAroundLookAtBehaviour : TargetObjectBehaviour
{
	[SerializeField]
	private float radius;

	[SerializeField]
	private Vector3 positionOffset = Vector3.zero;

	[SerializeField]
	private Vector3 lookatPositionOffset = Vector3.zero;

	[SerializeField]
	private float rotationDamping = 1f;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			float f = Mathf.Atan2(target.transform.position.x + positionOffset.x, target.transform.position.z + positionOffset.z);
			Vector3 vector = new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f));
			Vector3 vector2 = vector * radius + positionOffset;
			position = vector2;
			rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(target.position + lookatPositionOffset - base.transform.position), Time.deltaTime * rotationDamping);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}

	public void SetPositionOffset(Vector3 offset)
	{
		positionOffset = offset;
	}
}

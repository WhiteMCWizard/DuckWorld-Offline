using UnityEngine;

namespace SLAM.CameraSystem;

public class SecurityCamBehaviour : CameraBehaviour
{
	[SerializeField]
	private Transform location;

	[SerializeField]
	private Transform target;

	[SerializeField]
	private Vector3 targetOffset;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		position = location.position;
		Vector3 eulerAngles = Quaternion.LookRotation(target.position + targetOffset - base.transform.position).eulerAngles;
		Quaternion quaternion = Quaternion.Euler(eulerAngles);
		rotation = quaternion;
	}
}

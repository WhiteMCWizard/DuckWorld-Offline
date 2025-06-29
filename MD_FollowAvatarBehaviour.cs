using SLAM.CameraSystem;
using UnityEngine;

public class MD_FollowAvatarBehaviour : CameraBehaviour
{
	[SerializeField]
	public Transform target;

	[SerializeField]
	private Vector3 positionalOffset;

	[SerializeField]
	private Vector3 rotationalOffset;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			position = target.position + positionalOffset;
			rotation = Quaternion.Euler(rotationalOffset);
		}
		else
		{
			position = base.LocalTransform.position;
			rotation = base.LocalTransform.rotation;
		}
	}
}

using UnityEngine;

namespace SLAM.CameraSystem;

public class FollowLookAtBehaviour : TargetObjectBehaviour
{
	[SerializeField]
	private Vector3 positionOffset;

	[SerializeField]
	private Vector3 lookatPositionOffset;

	[SerializeField]
	private float rotationDamping;

	[SerializeField]
	private float positionDamping;

	protected override void Start()
	{
		base.Start();
		if (target == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("CameraTarget");
			target = gameObject.transform;
		}
	}

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			position = Vector3.Lerp(base.transform.position, target.position - target.forward * positionOffset.z + target.up * positionOffset.y + target.right * positionOffset.x, Time.deltaTime * positionDamping);
			rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(target.position + lookatPositionOffset - base.transform.position), Time.deltaTime * rotationDamping);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}

	public Vector3 GetPosOffset()
	{
		return positionOffset;
	}

	public void SetPosOffset(Vector3 offset)
	{
		positionOffset = offset;
	}

	public void WarpTo()
	{
		base.transform.parent.position = target.position - target.forward * positionOffset.z + target.up * positionOffset.y + target.right * positionOffset.x;
		base.transform.parent.rotation = Quaternion.LookRotation(target.position + lookatPositionOffset - base.transform.position);
	}
}

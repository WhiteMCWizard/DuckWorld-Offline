using SLAM.CameraSystem;
using UnityEngine;

public class SidescrollerBehaviour : CameraBehaviour
{
	[SerializeField]
	public Transform target;

	[SerializeField]
	private Vector3 positionDamping = Vector3.one;

	[SerializeField]
	private Vector3 distance;

	[SerializeField]
	private bool trackX = true;

	[SerializeField]
	private bool trackY = true;

	[SerializeField]
	private bool trackZ = true;

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			Vector3 vector = GetTarget(target.position);
			position.x = Mathf.Lerp(base.LocalTransform.position.x, vector.x, Time.deltaTime * positionDamping.x);
			position.y = Mathf.Lerp(base.LocalTransform.position.y, vector.y, Time.deltaTime * positionDamping.y);
			position.z = Mathf.Lerp(base.LocalTransform.position.z, vector.z, Time.deltaTime * positionDamping.z);
			rotation = base.LocalTransform.rotation;
		}
		else
		{
			position = base.LocalTransform.position;
			rotation = base.LocalTransform.rotation;
		}
	}

	private Vector3 GetTarget(Vector3 pos)
	{
		Vector3 result = pos - target.forward * distance.x + target.up * distance.y;
		if (!trackX)
		{
			result.x = base.LocalTransform.position.x;
		}
		if (!trackY)
		{
			result.y = base.LocalTransform.position.y;
		}
		if (trackZ)
		{
			result.z += distance.z;
		}
		return result;
	}

	public void WarpTo(Vector3 position)
	{
		base.LocalTransform.position = GetTarget(position);
	}

	public Vector3 GetDistance()
	{
		return distance;
	}

	public void SetDistance(Vector3 distance)
	{
		this.distance = distance;
	}
}

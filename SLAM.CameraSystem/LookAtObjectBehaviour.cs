using UnityEngine;

namespace SLAM.CameraSystem;

public class LookAtObjectBehaviour : TargetObjectBehaviour
{
	[SerializeField]
	private Vector2 offsetDistance;

	[SerializeField]
	private float rotationDamping;

	[SerializeField]
	private Vector3 rotationOffset;

	protected override void Start()
	{
		base.Start();
		if (target == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("CameraTarget");
			if (gameObject != null)
			{
				target = gameObject.transform;
			}
		}
	}

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null)
		{
			position = base.position;
			rotation = Quaternion.Slerp(base.transform.parent.rotation, Quaternion.LookRotation(target.position - base.transform.position), Time.deltaTime * rotationDamping);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}
}

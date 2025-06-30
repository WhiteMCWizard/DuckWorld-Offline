using SLAM.CameraSystem;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRgameCamera : TargetObjectBehaviour
{
	[SerializeField]
	private Vector2 offsetDistance;

	[SerializeField]
	private float rotationDamping;

	[SerializeField]
	private float positionDamping;

	[SerializeField]
	private Vector3 rotationOffset;

	private Vector3 currentVel;

	private Vector3 currentForwardVector;

	[SerializeField]
	private float smoothDampTime = 0.13f;

	private float forwardSmoothDampTime = 0.24f;

	[SerializeField]
	private bool SmoothingStarted;

	private Vector3 cameraForwardVector;

	protected override void Start()
	{
		base.Start();
		if (target == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("CameraTarget");
			if (gameObject != null)
			{
				target = gameObject.transform;
				cameraForwardVector = target.forward;
			}
		}
	}

	public override void GetPositionAndRotation(out Vector3 position, out Quaternion rotation)
	{
		if (target != null && SmoothingStarted)
		{
			Vector3 vector = target.forward;
			if (target.root.GetComponent<Rigidbody>() != null && target.root.GetComponent<Rigidbody>().velocity.sqrMagnitude > 2f)
			{
				vector = target.root.GetComponent<Rigidbody>().velocity.normalized;
			}
			cameraForwardVector = Vector3.SmoothDamp(cameraForwardVector, vector, ref currentForwardVector, forwardSmoothDampTime);
			Vector3 vector2 = target.position - vector * offsetDistance.x + target.up * offsetDistance.y;
			Vector3 vector3 = target.position + target.up * offsetDistance.y / 2f;
			Vector3 vector4 = Vector3.Normalize(vector2 - vector3);
			Ray ray = new Ray(vector3, vector4);
			if (Physics.Raycast(ray, out var hitInfo, Vector3.Distance(vector2, vector3) + 1f))
			{
				vector2 = vector3 + vector4 * (Vector3.Distance(hitInfo.point, vector3) - 0.5f);
			}
			position = Vector3.SmoothDamp(base.transform.position, vector2, ref currentVel, smoothDampTime, 99999f, Time.deltaTime);
			rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(vector + rotationOffset), Time.deltaTime * rotationDamping);
		}
		else
		{
			position = base.transform.position;
			rotation = base.transform.rotation;
		}
	}

	public void StartSmoothing()
	{
		SmoothingStarted = true;
	}

	public void WarpTo()
	{
		base.transform.parent.position = target.position - target.forward * offsetDistance.x + target.up * offsetDistance.y;
		base.transform.parent.rotation = Quaternion.LookRotation(target.forward + rotationOffset);
	}
}

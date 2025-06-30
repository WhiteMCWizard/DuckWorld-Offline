using UnityEngine;

namespace SLAM.Platformer;

public class P_Crate : MonoBehaviour
{
	private bool beingPushed;

	private Ray ray1;

	private Ray ray2;

	private Rigidbody rigidBody;

	private void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		if (!beingPushed && !rigidBody.isKinematic && rigidBody.IsSleeping())
		{
			rigidBody.isKinematic = true;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawLine(ray1.origin, ray1.origin + ray1.direction * 0.1f);
		Gizmos.DrawWireSphere(ray1.origin, 1f);
	}

	public void StartPushing()
	{
		beingPushed = true;
		rigidBody.isKinematic = false;
	}

	public void StopPushing()
	{
		beingPushed = false;
		if (CheckFloor())
		{
			rigidBody.Sleep();
		}
	}

	private bool CheckFloor()
	{
		Collider component = GetComponent<Collider>();
		ray1 = new Ray(component.bounds.min + Vector3.up * 0.1f, Vector3.down);
		ray2 = new Ray(component.bounds.min + Vector3.up * 0.1f + Vector3.right * component.bounds.size.x, Vector3.down);
		RaycastHit hitInfo;
		return Physics.Raycast(ray1, out hitInfo, 0.2f) && Physics.Raycast(ray2, out hitInfo, 0.2f);
	}
}

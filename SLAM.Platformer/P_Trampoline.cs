using UnityEngine;

namespace SLAM.Platformer;

public class P_Trampoline : P_Trigger
{
	[SerializeField]
	private float force = 1f;

	[SerializeField]
	private float angleModifier;

	private void OnDrawGizmos()
	{
		GizmosUtils.DrawArrow(base.transform.position, getReflectionAngle());
	}

	protected override void OnTriggerEnter(Collider other)
	{
		AudioController.Play("BC_web_bounce_01", base.transform);
		base.OnTriggerEnter(other);
		if (other.HasComponent<CC2DTriggerHelper>())
		{
			CC2DPlayer componentInParent = other.GetComponentInParent<CC2DPlayer>();
			componentInParent.AddForce(getReflectionAngle() * force);
			Animator component = GetComponent<Animator>();
			if (component != null)
			{
				component.SetTrigger("bounce");
			}
		}
	}

	private Vector3 getReflectionAngle()
	{
		return Quaternion.Euler(0f, 0f, angleModifier) * base.transform.up;
	}
}

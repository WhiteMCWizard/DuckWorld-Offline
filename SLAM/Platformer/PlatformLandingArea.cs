using UnityEngine;

namespace SLAM.Platformer;

public class PlatformLandingArea : P_Trigger
{
	private Transform player;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (other.transform.root.GetComponentInChildren<CC2DPlayer>() != null)
		{
			player = other.transform.parent;
			player.parent = base.transform;
		}
	}

	protected override void OnTriggerExit(Collider other)
	{
		base.OnTriggerExit(other);
		if (player.parent != null && player.parent.transform == base.transform)
		{
			player.parent = null;
		}
	}
}

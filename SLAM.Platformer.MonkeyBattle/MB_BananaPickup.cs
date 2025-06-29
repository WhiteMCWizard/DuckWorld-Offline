using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_BananaPickup : P_Pickup
{
	protected override void Start()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (other.HasComponent<MB_Flintheart>())
		{
			Object.Destroy(base.gameObject);
		}
	}
}

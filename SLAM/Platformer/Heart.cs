using UnityEngine;

namespace SLAM.Platformer;

public class Heart : P_Pickup
{
	private PlatformerGame game;

	protected override void Start()
	{
		base.Start();
		game = Object.FindObjectOfType<PlatformerGame>();
	}

	protected override void OnTriggerEnter(Collider other)
	{
		if (game.CollectHeart(this))
		{
			PickupCollectedEvent pickupCollectedEvent = new PickupCollectedEvent();
			pickupCollectedEvent.pickup = this;
			GameEvents.Invoke(pickupCollectedEvent);
		}
		base.OnTriggerEnter(other);
	}
}

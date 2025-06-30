using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_Feather : P_Pickup
{
	protected override void Start()
	{
		base.Start();
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		PickupCollectedEvent pickupCollectedEvent = new PickupCollectedEvent();
		pickupCollectedEvent.pickup = this;
		GameEvents.Invoke(pickupCollectedEvent);
	}
}

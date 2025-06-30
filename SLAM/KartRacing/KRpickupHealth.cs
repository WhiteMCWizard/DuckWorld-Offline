using SLAM.Platformer;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRpickupHealth : P_Pickup
{
	protected override void Start()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	protected override void OnTriggerEnter(Collider col)
	{
		KR_PhysicsKart component = col.GetComponent<KR_PhysicsKart>();
		if (component != null)
		{
			base.OnTriggerEnter(col);
			KR_PickupEvent kR_PickupEvent = new KR_PickupEvent();
			kR_PickupEvent.Kart = component;
			kR_PickupEvent.Pickup = this;
			GameEvents.InvokeAtEndOfFrame(kR_PickupEvent);
		}
	}
}

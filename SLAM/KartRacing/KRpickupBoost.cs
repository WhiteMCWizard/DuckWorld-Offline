using UnityEngine;

namespace SLAM.KartRacing;

public class KRpickupBoost : MonoBehaviour
{
	[SerializeField]
	private float boostForce = 100f;

	private void OnTriggerEnter(Collider col)
	{
		KR_PhysicsKart component = col.GetComponent<KR_PhysicsKart>();
		if (!(component == null))
		{
			KR_PickupBoostEvent kR_PickupBoostEvent = new KR_PickupBoostEvent();
			kR_PickupBoostEvent.Kart = component;
			kR_PickupBoostEvent.Pickup = this;
			GameEvents.InvokeAtEndOfFrame(kR_PickupBoostEvent);
			component.Push(-base.transform.forward * boostForce);
			if (component.GetType() == typeof(KR_HumanKart))
			{
				AudioController.Play("KR_pickup_turbo");
			}
		}
	}
}

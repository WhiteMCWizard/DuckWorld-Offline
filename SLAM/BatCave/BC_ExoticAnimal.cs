using System.Collections;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_ExoticAnimal : P_Pickup
{
	public enum ExoticAnimalType
	{
		Brown,
		Green,
		Blue
	}

	[SerializeField]
	private ExoticAnimalType type;

	[SerializeField]
	private PrefabSpawner particlesSpawner;

	[SerializeField]
	private Transform particlesDestination;

	public ExoticAnimalType Type => type;

	protected override void OnTriggerEnter(Collider other)
	{
		particlesSpawner.SpawnAt(particlesDestination.position);
		StartCoroutine(waitTillEndOfFrameAndFireEvent(other));
	}

	private IEnumerator waitTillEndOfFrameAndFireEvent(Collider other)
	{
		yield return new WaitForEndOfFrame();
		PickupCollectedEvent pickupCollectedEvent = new PickupCollectedEvent();
		pickupCollectedEvent.pickup = this;
		GameEvents.Invoke(pickupCollectedEvent);
		doCallBaseTriggerEnter(other);
	}

	private void doCallBaseTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}
}

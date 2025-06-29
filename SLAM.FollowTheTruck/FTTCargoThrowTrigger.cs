using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTCargoThrowTrigger : FTTInteractable<FTTTruckController>
{
	[SerializeField]
	private FTTCargoType cargoType;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(base.transform.position, Vector3.one);
		foreach (Transform item in base.transform)
		{
			Gizmos.DrawLine(base.transform.position, item.position);
			Gizmos.DrawSphere(item.position, 0.2f);
		}
	}

	public override void OnInteract(FTTTruckController truckController)
	{
		Transform target = ((base.transform.childCount <= 0) ? base.transform : base.transform.GetChild(Random.Range(0, base.transform.childCount)));
		FTTTrowCargoEvent fTTTrowCargoEvent = new FTTTrowCargoEvent();
		fTTTrowCargoEvent.Type = cargoType;
		fTTTrowCargoEvent.Target = target;
		GameEvents.Invoke(fTTTrowCargoEvent);
	}
}

using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTAnimatorTrigger : FTTInteractable<Animator>
{
	[SerializeField]
	private string triggerName;

	public override void OnInteract(Animator controller)
	{
		controller.SetTrigger(triggerName);
		if (controller.GetComponent<FTTTruckController>() != null)
		{
			AudioController.Stop("CTT_truck_drive_loop");
			AudioController.Play("CTT_truck_decelerate_off");
		}
	}
}

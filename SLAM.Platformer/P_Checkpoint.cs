using UnityEngine;

namespace SLAM.Platformer;

public class P_Checkpoint : P_Trigger
{
	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		CheckPointReachedEvent checkPointReachedEvent = new CheckPointReachedEvent();
		checkPointReachedEvent.checkpoint = this;
		GameEvents.Invoke(checkPointReachedEvent);
	}
}

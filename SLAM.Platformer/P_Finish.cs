using System.Collections;
using UnityEngine;

namespace SLAM.Platformer;

public class P_Finish : P_Trigger
{
	public Transform entrance;

	protected override void OnTriggerEnter(Collider other)
	{
		if (!hasBeenTriggered)
		{
			base.OnTriggerEnter(other);
			StartCoroutine(waitTillEndOfFrameAndFireEvent());
		}
	}

	private IEnumerator waitTillEndOfFrameAndFireEvent()
	{
		yield return new WaitForEndOfFrame();
		FinishReachedEvent finishReachedEvent = new FinishReachedEvent();
		finishReachedEvent.Finish = this;
		GameEvents.Invoke(finishReachedEvent);
	}
}

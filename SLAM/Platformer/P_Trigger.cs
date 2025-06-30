using UnityEngine;

namespace SLAM.Platformer;

public class P_Trigger : MonoBehaviour
{
	protected bool hasBeenTriggered;

	protected virtual void Start()
	{
		base.gameObject.layer = 14;
		if (GetComponent<Collider>() == null)
		{
			Debug.LogError("Error: No collider found on this trigger. OnTrigger events will never get called.");
		}
		else
		{
			GetComponent<Collider>().isTrigger = true;
		}
	}

	protected virtual void Update()
	{
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		hasBeenTriggered = true;
	}

	protected virtual void OnTriggerExit(Collider other)
	{
		hasBeenTriggered = false;
	}
}

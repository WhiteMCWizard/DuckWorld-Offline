using System.Collections.Generic;
using SLAM;
using SLAM.KartRacing;
using UnityEngine;

public class KR_Finish : MonoBehaviour
{
	private int nrOfKartPassed;

	private List<KR_KartBase> finishedKarts = new List<KR_KartBase>();

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider col)
	{
		KR_KartBase component = col.GetComponent<KR_KartBase>();
		if (component != null && !finishedKarts.Contains(component))
		{
			nrOfKartPassed++;
			finishedKarts.Add(component);
			KR_FinishCrossedEvent kR_FinishCrossedEvent = new KR_FinishCrossedEvent();
			kR_FinishCrossedEvent.Kart = component;
			kR_FinishCrossedEvent.PodiumPosition = nrOfKartPassed;
			GameEvents.InvokeAtEndOfFrame(kR_FinishCrossedEvent);
		}
	}
}

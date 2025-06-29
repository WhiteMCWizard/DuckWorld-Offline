using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTendGameTrigger : MonoBehaviour
{
	[SerializeField]
	private Transform truckEndPosition;

	private void OnTriggerEnter(Collider col)
	{
		if (!(col.GetComponent<ZTtruck>() == null))
		{
			ZooTransportGame.ZTTruckReachedEndEevent zTTruckReachedEndEevent = new ZooTransportGame.ZTTruckReachedEndEevent();
			zTTruckReachedEndEevent.TruckEndPosition = truckEndPosition.position;
			GameEvents.Invoke(zTTruckReachedEndEevent);
		}
	}
}

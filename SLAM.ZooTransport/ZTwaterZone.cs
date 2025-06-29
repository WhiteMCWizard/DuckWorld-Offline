using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTwaterZone : MonoBehaviour
{
	private void OnTriggerEnter(Collider col)
	{
		if (!(col.GetComponent<ZTtruck>() == null))
		{
			ZooTransportGame.instance.TumbledDown();
		}
	}
}

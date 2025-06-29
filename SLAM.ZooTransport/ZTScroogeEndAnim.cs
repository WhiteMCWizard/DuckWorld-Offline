using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTScroogeEndAnim : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Subscribe<ZooTransportGame.ZTGameEndEvent>(onGameEnd);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<ZooTransportGame.ZTGameEndEvent>(onGameEnd);
	}

	private void onGameEnd(ZooTransportGame.ZTGameEndEvent evt)
	{
		GetComponentInChildren<Animator>().SetTrigger("Cheer");
	}
}

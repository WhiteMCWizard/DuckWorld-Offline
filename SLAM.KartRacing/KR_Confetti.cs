using UnityEngine;

namespace SLAM.KartRacing;

public class KR_Confetti : MonoBehaviour
{
	[SerializeField]
	private PrefabSpawner confettiSpawner;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_FinishCrossedEvent>(onFinishCrossed);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_FinishCrossedEvent>(onFinishCrossed);
	}

	private void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		if (evt.Kart is KR_HumanKart && evt.PodiumPosition == 1 && confettiSpawner != null)
		{
			confettiSpawner.Spawn();
		}
	}
}

using System;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_HeartSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject heartPrefab;

	[SerializeField]
	private float radius = 12f;

	private float heartSpawnTime = 10f;

	private void OnEnable()
	{
		GameEvents.Subscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		GameEvents.Subscribe<PlayerHitEvent>(onPlayerHit);
		GameEvents.Subscribe<MonkeyBattleGame.GameEndedEvent>(onGameEndedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<MonkeyBattleGame.GameStartedEvent>(onGameStartedEvent);
		GameEvents.Unsubscribe<PlayerHitEvent>(onPlayerHit);
		GameEvents.Unsubscribe<MonkeyBattleGame.GameEndedEvent>(onGameEndedEvent);
	}

	private void onGameStartedEvent(MonkeyBattleGame.GameStartedEvent evt)
	{
		heartSpawnTime = evt.settings.heartRespawnInterval;
	}

	private void onGameEndedEvent(MonkeyBattleGame.GameEndedEvent evt)
	{
		base.enabled = false;
	}

	private void spawnNewHeartPrefab()
	{
		if (base.enabled)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(heartPrefab);
			gameObject.transform.position = getRandomPositionOnPath();
		}
	}

	private Vector3 getRandomPositionOnPath()
	{
		float f = (float)Math.PI * 2f * UnityEngine.Random.value;
		return new Vector3(Mathf.Sin(f) * radius, 0.5f, Mathf.Cos(f) * radius);
	}

	private void onPlayerHit(PlayerHitEvent evt)
	{
		Invoke("spawnNewHeartPrefab", heartSpawnTime);
	}
}

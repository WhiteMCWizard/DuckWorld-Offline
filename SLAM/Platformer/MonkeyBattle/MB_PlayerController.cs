using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_PlayerController : PlayerPathController
{
	[SerializeField]
	private float invulnerableDuration = 1f;

	private float invulnerableTimer;

	private void HitByBanana(MB_Banana obj)
	{
		Object.Destroy(obj.gameObject);
		GetComponent<PrefabSpawner>().SpawnOneAt<PrefabSpawner>(base.transform.position);
		if (Time.time > invulnerableTimer)
		{
			invulnerableTimer = Time.time + invulnerableDuration;
			animator.SetTrigger("hit");
			PlayerHitEvent playerHitEvent = new PlayerHitEvent();
			playerHitEvent.EnemyObject = obj.gameObject;
			GameEvents.Invoke(playerHitEvent);
		}
	}

	public Vector3 GetAimingPosition()
	{
		Vector3 position = base.transform.position;
		return position + Vector3.up / 2f;
	}
}

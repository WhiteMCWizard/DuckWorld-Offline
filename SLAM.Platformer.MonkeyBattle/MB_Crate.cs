using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_Crate : MB_DestructibleObstacle
{
	[SerializeField]
	private AudioClip hitSound;

	protected override void HitByBanana(MB_Banana banana)
	{
		AudioController.Play(hitSound.name, base.transform.position);
		GetComponent<PrefabSpawner>().SpawnOneAt<ParticleSystem>(base.transform.position);
		base.HitByBanana(banana);
	}
}

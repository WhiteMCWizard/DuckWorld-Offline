using UnityEngine;

namespace SLAM.Platformer;

public class P_Pit : P_Trigger
{
	[SerializeField]
	private PrefabSpawner deathParticlesSpawner;

	[SerializeField]
	private AudioClip deathAudioItem;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (deathParticlesSpawner != null)
		{
			Vector3 position = GetComponent<Collider>().ClosestPointOnBounds(other.transform.position);
			deathParticlesSpawner.SpawnAt(position);
		}
		if (deathAudioItem != null)
		{
			AudioController.Play(deathAudioItem.name, base.transform);
		}
		PlayerFallInWaterEvent playerFallInWaterEvent = new PlayerFallInWaterEvent();
		playerFallInWaterEvent.Water = this;
		GameEvents.Invoke(playerFallInWaterEvent);
	}
}

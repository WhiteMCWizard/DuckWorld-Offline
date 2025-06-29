using UnityEngine;

namespace SLAM.Platformer;

public class P_ParticlesAtImpact : P_Trigger
{
	[SerializeField]
	private PrefabSpawner particlesSpawner;

	[SerializeField]
	private AudioClip audioItem;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (particlesSpawner != null)
		{
			Vector3 position = GetComponent<Collider>().ClosestPointOnBounds(other.transform.position);
			particlesSpawner.SpawnAt(position);
		}
		if (audioItem != null)
		{
			AudioController.Play(audioItem.name, base.transform);
		}
	}
}

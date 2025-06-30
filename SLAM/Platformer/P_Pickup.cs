using UnityEngine;

namespace SLAM.Platformer;

public class P_Pickup : P_Trigger
{
	[SerializeField]
	protected PrefabSpawner pickupParticlesSpawner;

	[SerializeField]
	protected string pickupSound;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (pickupParticlesSpawner != null)
		{
			pickupParticlesSpawner.SpawnAt(base.transform.position);
		}
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(pickupSound);
		}
		Object.Destroy(base.gameObject);
	}
}

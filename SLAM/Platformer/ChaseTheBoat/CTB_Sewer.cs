using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

[RequireComponent(typeof(BoxCollider), typeof(PrefabSpawner))]
public class CTB_Sewer : P_Trigger
{
	[SerializeField]
	private PrefabSpawner prefabSpawner;

	private ParticleSystem[] waterJetParticles;

	private AudioObject audioItem;

	[SerializeField]
	private AudioClip deathAudioItem;

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (deathAudioItem != null)
		{
			AudioController.Play(deathAudioItem.name, base.transform);
		}
		PlayerHitEvent playerHitEvent = new PlayerHitEvent();
		playerHitEvent.EnemyObject = base.gameObject;
		GameEvents.Invoke(playerHitEvent);
	}

	protected override void Start()
	{
		base.Start();
		waterJetParticles = prefabSpawner.Spawn<ParticleSystem>();
		Turn(on: true);
	}

	public void Turn(bool on)
	{
		GetComponent<Collider>().enabled = on;
		if (on)
		{
			audioItem = AudioController.Play("CTB_water_spray_loop", base.transform);
		}
		else if (audioItem != null)
		{
			audioItem.Stop();
		}
		for (int i = 0; i < waterJetParticles.Length; i++)
		{
			waterJetParticles[i].enableEmission = on;
		}
	}
}

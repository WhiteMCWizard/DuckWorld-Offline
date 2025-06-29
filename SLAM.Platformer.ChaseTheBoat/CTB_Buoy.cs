using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_Buoy : P_Trigger
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private float jumpHeight = 4f;

	[SerializeField]
	private PrefabSpawner waterSplashParticlesSpawner;

	private CC2DPlayer player;

	protected override void Start()
	{
		base.Start();
		player = GameObject.Find("AvatarRoot").GetComponent<CC2DPlayer>();
	}

	protected override void OnTriggerEnter(Collider other)
	{
		Vector3 normalized = (player.transform.position - GetComponent<Collider>().bounds.center).normalized;
		float num = Vector3.Dot(Vector3.up, normalized);
		if (num > 0f)
		{
			base.OnTriggerEnter(other);
			if (waterSplashParticlesSpawner != null)
			{
				waterSplashParticlesSpawner.Spawn();
			}
			animator.ResetTrigger("Bounce");
			animator.SetTrigger("Bounce");
			player.Jump(jumpHeight);
			AudioController.Play("CTB_jump_on_buoy", base.transform.position);
		}
	}
}

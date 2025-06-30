using System.Collections;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class CTB_BreakablePier : P_Trigger
{
	[SerializeField]
	private float timeTillBreakage = 1f;

	[SerializeField]
	private float timeTillHitWater = 0.3f;

	[SerializeField]
	private bool isHighPier;

	[SerializeField]
	private bool getStuck = true;

	[SerializeField]
	private Collider smoothCollider;

	[SerializeField]
	private Collider stuckCollider;

	[SerializeField]
	private PrefabSpawner breakPierParticlesSpawner;

	[SerializeField]
	private PrefabSpawner hitWaterParticlesSpawner;

	private Animator animator;

	private bool triggered;

	private Ray ray;

	protected override void Start()
	{
		base.Start();
		animator = GetComponent<Animator>();
		stuckCollider.enabled = getStuck;
		smoothCollider.enabled = !getStuck;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
		if (!triggered)
		{
			triggered = true;
			StartCoroutine(BreakRoutine());
		}
	}

	private IEnumerator BreakRoutine()
	{
		Vector3 position = base.transform.position;
		if (breakPierParticlesSpawner != null)
		{
			breakPierParticlesSpawner.Spawn();
		}
		animator.SetTrigger("Warn");
		AudioController.Play("CTB_wood_breakable_crack", position);
		yield return new WaitForSeconds(timeTillBreakage);
		if (isHighPier)
		{
			animator.SetTrigger("Break2");
		}
		else
		{
			animator.SetTrigger("Break1");
		}
		AudioController.Play("CTB_wood_breakable_break", position);
		AudioController.Play("CTB_wood_breakable_hitWater", position);
		Collider collider = stuckCollider;
		bool flag = false;
		smoothCollider.enabled = flag;
		collider.enabled = flag;
		yield return new WaitForSeconds(timeTillHitWater);
		if (!(hitWaterParticlesSpawner != null))
		{
			yield break;
		}
		Transform fxTransform = null;
		foreach (Transform t in base.transform)
		{
			if (t.name == "FX_NULL")
			{
				fxTransform = t;
			}
		}
		if (fxTransform != null)
		{
			ray = new Ray(base.transform.position - Vector3.up * 0.5f, Vector3.down);
			if (Physics.Raycast(ray, out var hitInfo, 5f))
			{
				fxTransform.position = hitInfo.point;
			}
		}
		hitWaterParticlesSpawner.Spawn();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawRay(ray);
	}
}

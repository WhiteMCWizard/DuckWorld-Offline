using System;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZTwheelScript : MonoBehaviour
{
	[SerializeField]
	private WheelCollider wheelCollider;

	[SerializeField]
	private PrefabSpawner[] wheelFxSpawners;

	private float rotationPerSpeed;

	private Vector3 positionOffset;

	private bool hitGround;

	private ParticleSystem[] wheelParticleSystems;

	private float prevSuspension;

	private void Start()
	{
		rotationPerSpeed = 360f / ((float)Math.PI * 2f * wheelCollider.radius);
		positionOffset = base.transform.localPosition;
		wheelParticleSystems = new ParticleSystem[wheelFxSpawners.Length];
		for (int i = 0; i < wheelFxSpawners.Length; i++)
		{
			wheelParticleSystems[i] = wheelFxSpawners[i].SpawnOne<ParticleSystem>();
		}
	}

	public void ManualFixedUpdate(float speed)
	{
		base.transform.Rotate(Vector3.right, speed * rotationPerSpeed * Time.fixedDeltaTime);
	}

	public bool IsOnGround()
	{
		return hitGround;
	}

	private void FixedUpdate()
	{
		hitGround = wheelCollider.GetGroundHit(out var hit);
		float num = wheelCollider.radius;
		if (hitGround)
		{
			num = Vector3.Distance(wheelCollider.transform.position, hit.point) - wheelCollider.radius;
			Vector3 vector = -base.transform.parent.up * num;
			base.transform.localPosition = vector + positionOffset;
		}
		if (Mathf.Abs(prevSuspension - num) > 0.05f)
		{
			AudioController.Play("Truck_suspension");
		}
		prevSuspension = num;
		for (int i = 0; i < wheelParticleSystems.Length; i++)
		{
			wheelParticleSystems[i].enableEmission = hitGround;
		}
	}
}

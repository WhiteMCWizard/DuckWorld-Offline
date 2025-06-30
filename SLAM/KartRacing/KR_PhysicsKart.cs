using System.Collections;
using System.Collections.Generic;
using SLAM.Kart;
using SLAM.Layers;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_PhysicsKart : KR_KartBase
{
	[SerializeField]
	protected KRwheelScript[] steeringWheels;

	[SerializeField]
	protected KRwheelScript[] backWheels;

	[SerializeField]
	protected float turnSpeed;

	[SerializeField]
	protected ConstantForce downForce;

	[SerializeField]
	protected float downForceMagnitude = 200f;

	[SerializeField]
	protected AnimationCurve steerAngleAtVelocity;

	[SerializeField]
	protected float wheelBrakeForce = 400f;

	[SerializeField]
	protected float maxSpeedPushBackForce = 50f;

	[SerializeField]
	protected AnimationCurve maxPushBackCurve;

	[SerializeField]
	protected float maxSpeed = 25f;

	[SerializeField]
	protected KRKartSettings kartSettings;

	[SerializeField]
	protected float startRacePush = 10f;

	[SerializeField]
	private float respawnWhenDrivingTooSlowDuration = 1f;

	[SerializeField]
	private float respawnWhenDrivingTooSlowVelocity = 1f;

	[SerializeField]
	private float respawnAtKartRollAngle = 80f;

	[SerializeField]
	private PrefabSpawner invulnerabilityParticlesSpawner;

	[SerializeField]
	private Layer layerWithCollisions;

	[SerializeField]
	private Layer layerWithoutCollissions;

	[SerializeField]
	private KartSpawner spawner;

	protected bool isInAir;

	protected int inAirFrameCount;

	protected Transform CenterOfMassTransform;

	protected Transform InAirCenterOfMassTransform;

	protected float isDrivingTooSlowDuration;

	protected Rigidbody rigidBody;

	private bool wasHitThisFrame;

	public KartSpawner Spawner
	{
		get
		{
			return spawner;
		}
		set
		{
			spawner = value;
		}
	}

	protected float ForwardVelocity => base.transform.InverseTransformDirection(rigidBody.velocity).z;

	public Rigidbody RigidBody => rigidBody;

	protected override void Start()
	{
		base.Start();
		rigidBody = GetComponent<Rigidbody>();
		Physics.IgnoreLayerCollision(layerWithCollisions.LayerIndex, layerWithoutCollissions.LayerIndex);
		Physics.IgnoreLayerCollision(layerWithoutCollissions.LayerIndex, layerWithoutCollissions.LayerIndex);
		rigidBody.useGravity = true;
		CenterOfMassTransform = base.transform.FindChild("CenterOfMass");
		InAirCenterOfMassTransform = base.transform.FindChild("CenterOfMassInAir");
		rigidBody.centerOfMass = CenterOfMassTransform.localPosition;
		if (spawner == null)
		{
			spawner = GetComponentInChildren<KartSpawner>();
		}
		if (spawner != null)
		{
			kartSettings.Acceleration = spawner.Config.GetStat(KartSystem.ItemStat.Acceleration);
			kartSettings.TopSpeed = spawner.Config.GetStat(KartSystem.ItemStat.TopSpeed);
			kartSettings.Handling = spawner.Config.GetStat(KartSystem.ItemStat.Handling);
			kartSettings.SnowBoost = spawner.Config.HasSnowItem();
			kartSettings.OilBoost = spawner.Config.HasOilItem();
			spawner.SpawnKart();
			KartBodyAnchor componentInChildren = GetComponentInChildren<KartBodyAnchor>();
			if (componentInChildren != null && steeringWheels.Length >= 2)
			{
				foreach (Transform anchor in componentInChildren.GetAnchors(KartSystem.ItemCategory.Wheels))
				{
					if (anchor.name.Contains("FrontLeft"))
					{
						steeringWheels[0].SetWheelModel(anchor.GetChild(0), isLeftWheel: true);
					}
					if (anchor.name.Contains("FrontRight"))
					{
						steeringWheels[1].SetWheelModel(anchor.GetChild(0), isLeftWheel: false);
					}
				}
			}
			if (componentInChildren != null && backWheels.Length >= 2)
			{
				foreach (Transform anchor2 in componentInChildren.GetAnchors(KartSystem.ItemCategory.Wheels))
				{
					if (anchor2.name.Contains("BackLeft"))
					{
						backWheels[0].SetWheelModel(anchor2.GetChild(0), isLeftWheel: true);
					}
					if (anchor2.name.Contains("BackRight"))
					{
						backWheels[1].SetWheelModel(anchor2.GetChild(0), isLeftWheel: false);
					}
				}
			}
		}
		applyKartSettings(kartSettings);
		addObjectToLayer(base.gameObject, layerWithCollisions.LayerIndex, recursivly: true);
	}

	protected virtual void Update()
	{
		wasHitThisFrame = false;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		handleMovement();
		isInAir = true;
		for (int i = 0; i < steeringWheels.Length; i++)
		{
			if (((WheelCollider)steeringWheels[i].GetComponent<Collider>()).isGrounded)
			{
				isInAir = false;
			}
		}
		if (isInAir)
		{
			inAirFrameCount++;
		}
		else
		{
			inAirFrameCount = 0;
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameEvents.Subscribe<KR_GameOverEvent>(onGameOver);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameEvents.Unsubscribe<KR_GameOverEvent>(onGameOver);
	}

	protected virtual void OnCollisionEnter(Collision col)
	{
		if (wasHitThisFrame || isInvulnerable || !(fsm.CurrentState.Name == "Racing"))
		{
			return;
		}
		wasHitThisFrame = true;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		bool flag = false;
		bool dynamicObject = false;
		Rigidbody otherBody = null;
		ContactPoint[] contacts = col.contacts;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint contactPoint = contacts[i];
			if (contactPoint.otherCollider.GetComponent<Rigidbody>() != null && !contactPoint.otherCollider.GetComponent<Rigidbody>().isKinematic)
			{
				dynamicObject = true;
				otherBody = contactPoint.otherCollider.GetComponent<Rigidbody>();
			}
			if (!contactPoint.otherCollider.tag.Equals("Ramp") && contactPoint.thisCollider.transform == base.transform && !(contactPoint.otherCollider is WheelCollider))
			{
				num = Mathf.Max(num, col.relativeVelocity.magnitude);
				num2 = Mathf.Max(num2, num);
				flag = true;
			}
			if (contactPoint.thisCollider is WheelCollider && rigidBody.velocity.y < -8f)
			{
				num2 = Mathf.Max(num2, col.relativeVelocity.magnitude);
				flag = true;
			}
			if (contactPoint.thisCollider.name == "BottomCollider")
			{
				num2 = Mathf.Max(num2, col.relativeVelocity.magnitude);
				num3 += base.transform.InverseTransformPoint(contactPoint.point).x;
				flag = true;
			}
		}
		if (flag)
		{
			onHit(num2, num, num3, dynamicObject, otherBody);
		}
	}

	protected override void OnEnterRacing()
	{
		rigidBody.isKinematic = false;
		Push(base.transform.forward * startRacePush);
		base.OnEnterRacing();
	}

	protected override void WhileRacing()
	{
		base.WhileRacing();
		if (isRespawning || isInvulnerable)
		{
			return;
		}
		float num = base.transform.rotation.eulerAngles.z;
		if (num > 180f)
		{
			num -= 360f;
		}
		if (ForwardVelocity < respawnWhenDrivingTooSlowVelocity || Mathf.Abs(num) > respawnAtKartRollAngle)
		{
			isDrivingTooSlowDuration += Time.deltaTime;
			if (isDrivingTooSlowDuration > respawnWhenDrivingTooSlowDuration)
			{
				respawn(respawnDuration, shouldLoseHeart: false);
				isDrivingTooSlowDuration = 0f;
			}
		}
		if (layerWithoutCollissions.LayerIndex == base.gameObject.layer)
		{
			Collider[] collection = Physics.OverlapSphere(base.transform.position, 2f, layerWithCollisions.LayerIndex);
			if (collection.Count((Collider c) => c.HasComponent<KR_KartBase>()) == 0)
			{
				addObjectToLayer(base.gameObject, layerWithCollisions.LayerIndex, recursivly: true);
			}
		}
		if (ForwardVelocity > respawnWhenDrivingTooSlowVelocity && Mathf.Abs(num) < respawnAtKartRollAngle)
		{
			isDrivingTooSlowDuration = 0f;
		}
	}

	protected override void OnEnterFinished()
	{
		base.OnEnterFinished();
		Freeze();
	}

	public virtual void OnSurfaceMaterialChange(KRPhysicsMaterialType type)
	{
	}

	public void SlowDownKart(float strength)
	{
		float num = 8f;
		if (rigidBody.velocity.magnitude > num)
		{
			rigidBody.AddForce(-rigidBody.velocity.normalized * strength);
		}
	}

	public virtual void GoIntoSpin()
	{
		StartCoroutine(spinKart());
	}

	public virtual void Push(Vector3 force)
	{
		rigidBody.AddForce(force, ForceMode.Impulse);
	}

	public virtual void Freeze()
	{
		rigidBody.isKinematic = true;
	}

	public virtual void UnFreeze()
	{
		rigidBody.isKinematic = false;
	}

	private void addObjectToLayer(GameObject go, int layer, bool recursivly)
	{
		go.layer = layer;
		if (!recursivly)
		{
			return;
		}
		foreach (Transform item in go.transform)
		{
			addObjectToLayer(item.gameObject, layer, recursivly);
		}
	}

	private void handleMovement()
	{
		if (isInAir && inAirFrameCount > 3)
		{
			downForce.force = Vector3.zero;
			rigidBody.centerOfMass = InAirCenterOfMassTransform.localPosition;
		}
		else
		{
			downForce.force = new Vector3(0f, 0f - downForceMagnitude, 0f);
			rigidBody.centerOfMass = CenterOfMassTransform.localPosition;
		}
		KRwheelScript[] array = steeringWheels;
		foreach (KRwheelScript kRwheelScript in array)
		{
			float steerAngle = steerAngleAtVelocity.Evaluate(rigidBody.velocity.magnitude) * steer * turnSpeed;
			kRwheelScript.WheelCollider.steerAngle = steerAngle;
		}
		if (brake > 0.1f && !isInAir)
		{
			rigidBody.AddForce(-rigidBody.velocity.normalized * brake * wheelBrakeForce * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
		float value = rigidBody.velocity.magnitude - maxSpeed;
		value = Mathf.Clamp(value, -10f, 5f);
		value = maxPushBackCurve.Evaluate(value);
		if (value > 0.05f)
		{
			rigidBody.AddForce(-rigidBody.velocity.normalized * maxSpeedPushBackForce * value * Time.fixedDeltaTime);
		}
	}

	protected virtual void onHit(float highestForce, float highestForceOnBody, float sideImpactDirection, bool dynamicObject, Rigidbody otherBody)
	{
		if (highestForceOnBody > impactForceToLoseHeart && !dynamicObject)
		{
			kartHitParticlesSpawner.Spawn();
			respawn(respawnDuration, shouldLoseHeart: true);
			AudioController.Play("KR_collision_max", base.transform);
		}
		if (highestForceOnBody > lowImpactThreshold)
		{
			characterAnimator.SetTrigger("Impact");
		}
		else if (!Mathf.Approximately(0f, sideImpactDirection))
		{
			if (sideImpactDirection > 0f)
			{
				characterAnimator.SetTrigger("LookRight");
			}
			else if (sideImpactDirection < 0f)
			{
				characterAnimator.SetTrigger("LookLeft");
			}
		}
	}

	protected void applyKartSettings(KRKartSettings kartSettings)
	{
		applyKartSettings(kartSettings, 1f);
	}

	protected void applyKartSettings(KRKartSettings kartSettings, float cheatMultiplier)
	{
		turnSpeed = Mathf.Lerp(kartSettings.MinTurnSpeed, kartSettings.MaxTurnSpeed, kartSettings.Handling);
		turnSpeed *= cheatMultiplier * 1.2f;
		maxSpeed = Mathf.Lerp(kartSettings.MinTopSpeed, kartSettings.MaxTopSpeed, kartSettings.TopSpeed);
		maxSpeed *= cheatMultiplier;
		downForceMagnitude = Mathf.Lerp(kartSettings.MinDownForceMagnitude, kartSettings.MaxDownForceMagnitude, kartSettings.Acceleration);
		downForceMagnitude *= cheatMultiplier;
		startRacePush = Mathf.Lerp(kartSettings.MinStartPush, kartSettings.MaxStartPush, kartSettings.Acceleration);
		maxSpeedPushBackForce = Mathf.Lerp(kartSettings.MinTopSpeedPushBack, kartSettings.MaxTopSpeedPushBack, kartSettings.Acceleration);
		KRwheelScript[] array = steeringWheels;
		foreach (KRwheelScript kRwheelScript in array)
		{
			kRwheelScript.SetKartSettings(kartSettings, this);
		}
	}

	private IEnumerator spinKart()
	{
		if (rigidBody.velocity.sqrMagnitude > 64f)
		{
			rigidBody.AddForce(-rigidBody.velocity.normalized * maxSpeedPushBackForce);
		}
		float spinDuration = 0.85f + Random.Range(-0.035f, -0.035f);
		float timer = 0f;
		KRwheelScript[] componentsInChildren = GetComponentsInChildren<KRwheelScript>();
		foreach (KRwheelScript wheel in componentsInChildren)
		{
			wheel.EnterOilPatch();
		}
		while (timer < spinDuration)
		{
			yield return new WaitForFixedUpdate();
			timer += Time.fixedDeltaTime;
			rigidBody.AddRelativeTorque(new Vector3(0f, 9999f, 0f), ForceMode.Force);
		}
		KRwheelScript[] componentsInChildren2 = GetComponentsInChildren<KRwheelScript>();
		foreach (KRwheelScript wheel2 in componentsInChildren2)
		{
			wheel2.ExitOilPatch();
		}
	}

	protected virtual void respawn(float inSeconds, bool shouldLoseHeart)
	{
		StartCoroutine(waitAndRespawn(inSeconds));
		if (shouldLoseHeart)
		{
			loseHeart();
		}
	}

	private IEnumerator waitAndRespawn(float duration)
	{
		Freeze();
		isRespawning = true;
		yield return new WaitForSeconds(duration);
		KR_Waypoint nearestWaypoint = currentTrack.GetNearestWayPoint(base.transform);
		List<Vector3> points = new List<Vector3>(new Vector3[3]
		{
			nearestWaypoint.Center.position,
			nearestWaypoint.Left.position,
			nearestWaypoint.Right.position
		});
		Vector3 safeSpot = nearestWaypoint.Center.position;
		foreach (Vector3 point in points)
		{
			Collider[] hitColliders = Physics.OverlapSphere(point, 1f, layerWithCollisions.LayerIndex);
			if (hitColliders.Count((Collider c) => c.HasComponent<KR_KartBase>()) == 0)
			{
				safeSpot = point;
				break;
			}
		}
		base.transform.position = safeSpot;
		base.transform.rotation = nearestWaypoint.Center.rotation;
		UnFreeze();
		rigidBody.velocity = Vector3.zero;
		isRespawning = false;
		beInvulnerable(invulnerabilityDuration);
		addObjectToLayer(base.gameObject, layerWithoutCollissions.LayerIndex, recursivly: true);
	}

	protected virtual void beInvulnerable(float duration)
	{
		StartCoroutine(doInvulnerableRoutine(invulnerabilityDuration));
	}

	private IEnumerator doInvulnerableRoutine(float duration)
	{
		isInvulnerable = true;
		GameObject v = invulnerabilityParticlesSpawner.Spawn();
		yield return new WaitForSeconds(duration);
		Object.Destroy(v);
		isInvulnerable = false;
	}

	protected override void loseHeart()
	{
		base.loseHeart();
	}

	private void onGameOver(KR_GameOverEvent evt)
	{
		Freeze();
	}
}

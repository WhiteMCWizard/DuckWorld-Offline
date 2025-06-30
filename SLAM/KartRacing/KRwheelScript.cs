using System;
using System.Collections;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRwheelScript : MonoBehaviour
{
	[SerializeField]
	private Transform wheelModel;

	private ParticleSystem currentParticle;

	[SerializeField]
	private PrefabSpawner dirtParticleSpawner;

	private ParticleSystem dirtParticle;

	[SerializeField]
	private PrefabSpawner mudParticleSpawner;

	private ParticleSystem mudParticle;

	[SerializeField]
	private PrefabSpawner oilParticleSpawner;

	private ParticleSystem oilParticle;

	[SerializeField]
	private PrefabSpawner snowParticleSpawner;

	private ParticleSystem snowParticle;

	[SerializeField]
	private PrefabSpawner iceParticleSpawner;

	private ParticleSystem iceParticle;

	private WheelCollider wheelCollider;

	private WheelFrictionCurve wfcfCurrent;

	private WheelFrictionCurve wfcsCurrent;

	private KRKartSettings kartSettings;

	private bool overwriteGroundMaterial;

	[SerializeField]
	private Vector3 WheelDriveRotationAxis = new Vector3(1f, 0f, 0f);

	private KR_PhysicsKart kart;

	private float maxBrakePower = 3000f;

	private KRPhysicsMaterialType currentMaterial;

	private float rotationPerSpeed;

	private Rigidbody kartRigidBody;

	public WheelCollider WheelCollider => wheelCollider;

	public bool OverwriteGroundMaterial
	{
		get
		{
			return overwriteGroundMaterial;
		}
		set
		{
			overwriteGroundMaterial = value;
		}
	}

	private void Awake()
	{
		wheelCollider = GetComponent<WheelCollider>();
		wheelCollider.motorTorque = 0.0001f;
		kartSettings = new KRKartSettings();
		currentMaterial = KRPhysicsMaterialType.Ice;
		currentParticle = null;
		rotationPerSpeed = 360f / ((float)Math.PI * 2f * wheelCollider.radius);
		kartRigidBody = GetComponentInParent<Rigidbody>();
	}

	public void SetKartSettings(KRKartSettings settings, KR_PhysicsKart kart)
	{
		kartSettings = settings;
		this.kart = kart;
	}

	private void FixedUpdate()
	{
		wheelCollider.GetGroundHit(out var hit);
		if (currentParticle != null)
		{
			currentParticle.enableEmission = wheelCollider.isGrounded;
		}
		if (!(hit.collider != null) || overwriteGroundMaterial)
		{
			return;
		}
		KRWheelPhysicsMaterial component = hit.collider.GetComponent<KRWheelPhysicsMaterial>();
		if (!(component != null))
		{
			return;
		}
		KRWheelPhysicsMaterialSettings settingsForMaterial = KRWheelPhysicsMaterialSettings.GetSettingsForMaterial(component.MaterialType);
		UpdateSurfaceMaterial(settingsForMaterial.MaterialType);
		if (settingsForMaterial.MaterialType == KRPhysicsMaterialType.Oil)
		{
			if (!kartSettings.OilBoost && kart != null)
			{
				kart.GoIntoSpin();
			}
		}
		else if (kartSettings != null)
		{
			SetForwardStiffness(settingsForMaterial.ForwardStiffness * (kartSettings.GetBoostForMaterial(settingsForMaterial.MaterialType) + kartSettings.Handling));
			SetSidewaysStiffness(settingsForMaterial.SidewaysStiffness * (kartSettings.GetBoostForMaterial(settingsForMaterial.MaterialType) + kartSettings.Handling));
			if (kart != null && settingsForMaterial.BrakePower > 0f)
			{
				kart.SlowDownKart(settingsForMaterial.BrakePower * maxBrakePower * Time.fixedDeltaTime);
			}
		}
	}

	public void UpdateSurfaceMaterial(KRPhysicsMaterialType type)
	{
		if (currentMaterial != type)
		{
			if (currentParticle != null)
			{
				currentParticle.enableEmission = false;
			}
			currentMaterial = type;
			if (kart != null)
			{
				kart.OnSurfaceMaterialChange(currentMaterial);
			}
			switch (currentMaterial)
			{
			case KRPhysicsMaterialType.Dirt:
				currentParticle = dirtParticle;
				break;
			case KRPhysicsMaterialType.Mud:
				currentParticle = mudParticle;
				break;
			case KRPhysicsMaterialType.Oil:
				currentParticle = oilParticle;
				break;
			case KRPhysicsMaterialType.Snow:
				currentParticle = snowParticle;
				break;
			case KRPhysicsMaterialType.Ice:
				currentParticle = iceParticle;
				break;
			default:
				currentParticle = dirtParticle;
				break;
			}
		}
	}

	public void EnterOilPatch()
	{
		overwriteGroundMaterial = true;
		UpdateSurfaceMaterial(KRPhysicsMaterialType.Oil);
		SetForwardStiffness(0f);
		SetSidewaysStiffness(0f);
	}

	public void ExitOilPatch()
	{
		StartCoroutine(ReturnGrip());
	}

	public IEnumerator ReturnGrip()
	{
		float timer = 0f;
		float duration = 0.8f;
		while (timer < duration)
		{
			timer += Time.deltaTime;
			yield return null;
			SetForwardStiffness(Mathf.Lerp(0f, 1f, timer / duration));
			SetSidewaysStiffness(Mathf.Lerp(0f, 1f, timer / duration));
		}
		overwriteGroundMaterial = false;
	}

	private void LateUpdate()
	{
		if (wheelModel != null)
		{
			float a = kartRigidBody.velocity.magnitude * rotationPerSpeed * Time.deltaTime;
			a = Mathf.Min(a, 180f * (Time.deltaTime * 12f));
			wheelModel.transform.Rotate(WheelDriveRotationAxis, a);
		}
	}

	public void SetWheelModel(Transform wheelModel, bool isLeftWheel)
	{
		this.wheelModel = wheelModel;
		if (dirtParticleSpawner != null)
		{
			ParentToWheel(dirtParticleSpawner.gameObject);
			dirtParticle = dirtParticleSpawner.SpawnOne<ParticleSystem>();
			dirtParticle.enableEmission = false;
			if (isLeftWheel)
			{
				ResetYRotation(dirtParticle.transform);
			}
		}
		if (mudParticleSpawner != null)
		{
			ParentToWheel(mudParticleSpawner.gameObject);
			mudParticle = mudParticleSpawner.SpawnOne<ParticleSystem>();
			mudParticle.enableEmission = false;
			if (isLeftWheel)
			{
				ResetYRotation(mudParticle.transform);
			}
		}
		if (oilParticleSpawner != null)
		{
			ParentToWheel(oilParticleSpawner.gameObject);
			oilParticle = oilParticleSpawner.SpawnOne<ParticleSystem>();
			oilParticle.enableEmission = false;
			if (isLeftWheel)
			{
				ResetYRotation(oilParticle.transform);
			}
		}
		if (snowParticleSpawner != null)
		{
			ParentToWheel(snowParticleSpawner.gameObject);
			snowParticle = snowParticleSpawner.SpawnOne<ParticleSystem>();
			snowParticle.enableEmission = false;
			if (isLeftWheel)
			{
				ResetYRotation(snowParticle.transform);
			}
		}
		if (iceParticleSpawner != null)
		{
			ParentToWheel(iceParticleSpawner.gameObject);
			iceParticle = iceParticleSpawner.SpawnOne<ParticleSystem>();
			iceParticle.enableEmission = false;
			if (isLeftWheel)
			{
				ResetYRotation(iceParticle.transform);
			}
		}
	}

	private void ParentToWheel(GameObject spawner)
	{
		spawner.transform.parent = wheelModel.parent;
		Vector3 localPosition = spawner.transform.localPosition;
		localPosition.x = 0f;
		localPosition.z = 0f;
		spawner.transform.localPosition = localPosition;
	}

	private void ResetYRotation(Transform t)
	{
		Vector3 eulerAngles = t.localRotation.eulerAngles;
		eulerAngles.y = 0f;
		t.localRotation = Quaternion.Euler(eulerAngles);
	}

	private void SetForwardStiffness(float stiffness)
	{
		wfcsCurrent = wheelCollider.forwardFriction;
		wfcsCurrent.stiffness = stiffness;
		wheelCollider.forwardFriction = wfcsCurrent;
	}

	private void SetSidewaysStiffness(float stiffness)
	{
		wfcsCurrent = wheelCollider.sidewaysFriction;
		wfcsCurrent.stiffness = stiffness;
		wheelCollider.sidewaysFriction = wfcsCurrent;
	}
}

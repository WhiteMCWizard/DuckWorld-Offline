using System.Collections;
using SLAM.Avatar;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_HumanKart : KR_PhysicsKart
{
	[SerializeField]
	private float extraBumpForce = 9999f;

	[SerializeField]
	private AvatarSpawn avatarSpawner;

	private AudioObject surfaceLoopAudio;

	private AudioObject drivingBumpLoopAudio;

	private AudioObject passByLoopAudio;

	private AudioObject flyingLoopAudio;

	private AudioObject steerScrapeLoopAudio;

	private AudioObject kartSpinAudio;

	private float drivingBumpStartVolume;

	private float steerScrapeStartVolume;

	public override int HeartsLeft
	{
		get
		{
			return base.HeartsLeft;
		}
		protected set
		{
			if (base.HeartsLeft != value)
			{
				if (value <= base.HeartsLeft || base.HeartsLeft < 3)
				{
					KR_HumanHeartCountChangedEvent kR_HumanHeartCountChangedEvent = new KR_HumanHeartCountChangedEvent();
					kR_HumanHeartCountChangedEvent.Kart = this;
					kR_HumanHeartCountChangedEvent.NewHeartCount = value;
					kR_HumanHeartCountChangedEvent.OldHeartCount = base.HeartsLeft;
					GameEvents.InvokeAtEndOfFrame(kR_HumanHeartCountChangedEvent);
				}
				base.HeartsLeft = value;
				if (base.HeartsLeft <= 0)
				{
					StartCoroutine(doGameOverRoutine());
				}
			}
		}
	}

	public override int RacePosition
	{
		get
		{
			return base.RacePosition;
		}
		set
		{
			if (value != base.RacePosition)
			{
				KR_HumanRacePositionChangedEvent kR_HumanRacePositionChangedEvent = new KR_HumanRacePositionChangedEvent();
				kR_HumanRacePositionChangedEvent.Kart = this;
				kR_HumanRacePositionChangedEvent.OldRacePosition = base.RacePosition;
				kR_HumanRacePositionChangedEvent.NewRacePosition = value;
				GameEvents.Invoke(kR_HumanRacePositionChangedEvent);
				base.RacePosition = value;
			}
		}
	}

	protected override void Start()
	{
		avatarSpawner.SpawnAvatar();
		base.Start();
		Transform parent = base.transform.FindChildRecursively("KS_Pivot_AvatarPosition");
		GameObject gameObject = characterAnimator.gameObject;
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		characterAnimator.SetTrigger("Activate");
	}

	protected override void WhileRacing()
	{
		base.WhileRacing();
		steer = SLAMInput.Provider.GetAxis("Horizontal");
		if (SLAMInput.Provider.GetButton(SLAMInput.Button.Down))
		{
			brake = 1f;
		}
		else if (SLAMInput.Provider.GetButton(SLAMInput.Button.UpOrAction))
		{
			brake = 0f;
		}
		else
		{
			brake = 0.5f;
		}
		brake = SLAMInput.Provider.GetAxis("Vertical") * -0.5f + 0.5f;
		float forwardVelocity = base.ForwardVelocity;
		float num = 20f;
		float num2 = 5f;
		float num3 = Mathf.Clamp01((forwardVelocity - num2) / num);
		passByLoopAudio.volume = num3;
		if (isInAir)
		{
			inAirFrameCount++;
			if (inAirFrameCount > 5)
			{
				num3 = 0f;
			}
		}
		else
		{
			inAirFrameCount = 0;
		}
		if (surfaceLoopAudio != null)
		{
			surfaceLoopAudio.volume = ((inAirFrameCount <= 5) ? Mathf.Clamp01(forwardVelocity / 10f * 0.5f + 0.5f) : 0f);
			surfaceLoopAudio.pitch = surfaceLoopAudio.volume;
		}
		flyingLoopAudio.volume = ((!((float)inAirFrameCount * Time.deltaTime > 1f)) ? 0f : Mathf.Clamp01((float)inAirFrameCount * Time.deltaTime - 1f));
		drivingBumpLoopAudio.volume = num3 * drivingBumpStartVolume;
		steerScrapeLoopAudio.volume = ((inAirFrameCount <= 5) ? (Mathf.Abs(steer) * steerScrapeStartVolume) : 0f);
	}

	public override void OnSurfaceMaterialChange(KRPhysicsMaterialType type)
	{
		base.OnSurfaceMaterialChange(type);
		if (surfaceLoopAudio != null && surfaceLoopAudio.IsPlaying())
		{
			surfaceLoopAudio.Stop();
		}
		switch (type)
		{
		case KRPhysicsMaterialType.Dirt:
			surfaceLoopAudio = AudioController.Play("KR_surface_dirt_loop");
			break;
		case KRPhysicsMaterialType.Mud:
			surfaceLoopAudio = AudioController.Play("KR_surface_mud_loop");
			break;
		case KRPhysicsMaterialType.Snow:
			surfaceLoopAudio = AudioController.Play("KR_surface_snow_loop");
			break;
		case KRPhysicsMaterialType.Ice:
			AudioController.Play("KR_kart_ice_start");
			surfaceLoopAudio = AudioController.Play("KR_surface_ice_loop");
			break;
		case KRPhysicsMaterialType.Wood:
			surfaceLoopAudio = AudioController.Play("KR_surface_wood_loop");
			break;
		case KRPhysicsMaterialType.Oil:
			break;
		}
	}

	public override void GoIntoSpin()
	{
		base.GoIntoSpin();
		AudioController.Play("KR_surface_oil");
	}

	public void StopAudio()
	{
		if (surfaceLoopAudio != null && surfaceLoopAudio.subItem != null)
		{
			surfaceLoopAudio.Stop();
		}
		if (drivingBumpLoopAudio != null && drivingBumpLoopAudio.subItem != null)
		{
			drivingBumpLoopAudio.Stop();
		}
		if (steerScrapeLoopAudio != null && steerScrapeLoopAudio.subItem != null)
		{
			steerScrapeLoopAudio.Stop();
		}
	}

	protected override void onPickUp(KR_PickupEvent pickup)
	{
		base.onPickUp(pickup);
		if (pickup.Kart == this)
		{
			HeartsLeft = Mathf.Clamp(HeartsLeft + 1, 0, 3);
		}
	}

	protected override void onHit(float highestForce, float highestForceOnBody, float sideImpactDirection, bool dynamicObject, Rigidbody otherBody)
	{
		base.onHit(highestForce, highestForceOnBody, sideImpactDirection, dynamicObject, otherBody);
		float num = 0.25f;
		if (highestForce > highImpactThreshold)
		{
			AudioController.Play("Crate_impact_large_random");
			num = 1f;
		}
		else if (highestForce > mediumImpactThreshold)
		{
			AudioController.Play("Crate_impact_medium_random");
			num = 0.75f;
		}
		else if (highestForce > lowImpactThreshold)
		{
			AudioController.Play("Crate_impact_small_random");
			num = 0.25f;
		}
		if (dynamicObject && otherBody != null)
		{
			otherBody.AddExplosionForce(num * extraBumpForce, base.transform.position, 36f, 1.4f, ForceMode.Impulse);
		}
	}

	protected override void onStartRace(KR_StartRaceEvent evt)
	{
		base.onStartRace(evt);
		surfaceLoopAudio = AudioController.Play("KR_surface_dirt_loop");
		surfaceLoopAudio.stopAfterFadeOut = true;
		drivingBumpLoopAudio = AudioController.Play("KR_driving_kartBumps_loop");
		drivingBumpStartVolume = drivingBumpLoopAudio.volume;
		passByLoopAudio = AudioController.Play("KR_highSpeed_passby_loop");
		flyingLoopAudio = AudioController.Play("KR_fly_loop");
		steerScrapeLoopAudio = AudioController.Play("KR_driving_steerScrape_loop");
		steerScrapeStartVolume = steerScrapeLoopAudio.volume;
	}

	public override void Push(Vector3 force)
	{
		base.Push(force);
	}

	private IEnumerator doGameOverRoutine()
	{
		Freeze();
		yield return new WaitForSeconds(respawnDuration);
		GameEvents.InvokeAtEndOfFrame(new KR_GameOverEvent());
	}
}

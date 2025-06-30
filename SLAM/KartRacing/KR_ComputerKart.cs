using System;
using System.Collections.Generic;
using SLAM.Avatar;
using SLAM.Kart;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_ComputerKart : KR_PhysicsKart
{
	public enum DrivingStyleTypes
	{
		aggresive,
		normal,
		passive
	}

	[Serializable]
	public class DrivingStyle
	{
		private DrivingStyleTypes type;

		private float brake;

		private float speed;

		private float cheatFactor;

		public DrivingStyleTypes Type => type;

		public float Brake => brake;

		public float Speed => speed;

		public float CheatFactor => cheatFactor;

		public DrivingStyle(DrivingStyleTypes type, float brake, float speed, float cheatFactor)
		{
			this.type = type;
			this.brake = brake;
			this.speed = speed;
			this.cheatFactor = cheatFactor;
		}
	}

	private const float STEER_INTERVAL_HARD = 0.6f;

	private const float STEER_INTERVAL_SOFT = 0.3f;

	private const float BRAKE_INTERVAL_HARD = 0.2f;

	private const float STEER_ANIMATION_INTERVAL = 0.1f;

	private const float ALIGNMENT_THRESHOLD = 5f;

	private const float SEPARATION_THRESHOLD = 2.5f;

	private const float COHESION_THRESHOLD = 10f;

	private const float DISTANCE_TO_WAYPOINT_SWITCHING = 2.3f;

	[SerializeField]
	private AnimationCurve brakeCurve;

	[SerializeField]
	private float rubberBanding = 1f;

	[SerializeField]
	private float baseDifficulty = 1f;

	[SerializeField]
	private CustomAvatarSpawn avatarSpawner;

	private KR_KartBase player;

	private KR_KartBase[] opponents;

	private KR_AISettings settings;

	private KR_Waypoint prevTargetWayPoint;

	private KR_Waypoint targetWayPoint;

	private KR_Waypoint nextTargetWayPoint;

	private List<DrivingStyle> drivingStyles;

	private DrivingStyle kartDrivingStyle;

	private float kartAngleToWPL;

	private float kartAngleToWPR;

	private float kartAlignment;

	private float steerAnim;

	private float curSteerAnimForDamping;

	private AudioObject npcAudio;

	private Vector3 alignmentVector;

	private Vector3 cohesionVector;

	private Vector3 separationVector;

	public CustomAvatarSpawn AvatarSpawner
	{
		get
		{
			return avatarSpawner;
		}
		set
		{
			avatarSpawner = value;
		}
	}

	private bool isDrivingTooClose => getTooCloseOpponents().Length > 0;

	private bool isInFrontOfWayPoint
	{
		get
		{
			if (prevTargetWayPoint == null)
			{
				return true;
			}
			Vector3 normalized = (targetWayPoint.Left.position - prevTargetWayPoint.Left.position).normalized;
			Vector3 normalized2 = (targetWayPoint.Right.position - prevTargetWayPoint.Right.position).normalized;
			Vector3 normalized3 = (targetWayPoint.Left.position - base.transform.position).normalized;
			Vector3 normalized4 = (targetWayPoint.Right.position - base.transform.position).normalized;
			float num = MathUtilities.AngleSigned(normalized3, normalized, Vector3.up);
			float num2 = MathUtilities.AngleSigned(normalized4, normalized2, Vector3.up);
			return num > 0f && num2 < 0f;
		}
	}

	private bool isAligned => kartAlignment >= -5f && kartAlignment <= 5f;

	protected override void Start()
	{
		base.Start();
		drivingStyles = new List<DrivingStyle>();
		DrivingStyle item = new DrivingStyle(DrivingStyleTypes.aggresive, 0.8f, 1f, 0.08f);
		DrivingStyle item2 = new DrivingStyle(DrivingStyleTypes.normal, 0.8f, 0.9f, 0f);
		DrivingStyle item3 = new DrivingStyle(DrivingStyleTypes.passive, 0.9f, 0.8f, -0.08f);
		drivingStyles.Add(item);
		drivingStyles.Add(item2);
		drivingStyles.Add(item3);
		Transform parent = base.transform.FindChildRecursively("KS_Pivot_AvatarPosition");
		if (settings.AvatarPrefab != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(settings.AvatarPrefab);
			gameObject.transform.parent = parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			characterAnimator = gameObject.GetComponent<Animator>();
			characterAnimator.SetTrigger("Activate");
		}
		else
		{
			AvatarSpawner = GetComponentInChildren<CustomAvatarSpawn>();
			AvatarSpawner.SetConfiguration(settings.Avatar);
			GameObject gameObject2 = AvatarSpawner.SpawnAvatar();
			gameObject2.transform.parent = parent;
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
		}
	}

	protected override void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		base.onFinishCrossed(evt);
		if (evt.Kart == this)
		{
			npcAudio.FadeOut(1f);
		}
	}

	protected override void onStartRace(KR_StartRaceEvent evt)
	{
		base.onStartRace(evt);
		player = evt.Karts[0];
		opponents = evt.Karts.Where((KR_KartBase o) => o != this && o != evt.Karts[0]).ToArray();
		targetWayPoint = currentTrack.GetNearestWayPoint(base.gameObject.transform);
		nextTargetWayPoint = getNextTargetPoint(targetWayPoint);
		kartDrivingStyle = drivingStyles.FirstOrDefault((DrivingStyle s) => s.Type == DrivingStyleTypes.normal);
		npcAudio = AudioController.Play("KR_npc_kart_loop", base.transform);
	}

	protected override void Update()
	{
		base.Update();
		if (targetWayPoint != null && !isInAir)
		{
			Vector3 v = targetWayPoint.Left.position - base.transform.position;
			Vector3 v2 = targetWayPoint.Right.position - base.transform.position;
			kartAngleToWPL = MathUtilities.AngleSigned(base.transform.forward, v, Vector3.up);
			kartAngleToWPR = MathUtilities.AngleSigned(base.transform.forward, v2, Vector3.up);
			kartAlignment = MathUtilities.AngleSigned(base.transform.forward, alignmentVector, Vector3.up);
			kartAlignment = Mathf.Clamp(kartAlignment, kartAngleToWPL, kartAngleToWPR);
			updateFlockingVectors();
			updateWayPoint();
			updateDrivingStyle();
			updateSteer();
			updateBrake();
		}
	}

	protected override void OnEnterFinished()
	{
		base.OnEnterFinished();
		targetWayPoint = null;
	}

	protected override void WhileRacing()
	{
		base.WhileRacing();
		float target = steerAnim + ((steer > -0.1f && steer < 0.1f) ? 0f : ((!(steerAnim < steer)) ? (-0.1f) : 0.1f));
		steerAnim = Mathf.SmoothDamp(steerAnim, target, ref curSteerAnimForDamping, 0.1f);
		characterAnimator.SetFloat("Steer", Mathf.Clamp(steerAnim, -1f, 1f));
		float forwardVelocity = base.ForwardVelocity;
		float num = 25f;
		float num2 = 10f;
		float volume = Mathf.Clamp01((forwardVelocity - num2) / num);
		if (isInAir)
		{
			inAirFrameCount++;
			if (inAirFrameCount > 5)
			{
				volume = 0f;
			}
		}
		else
		{
			inAirFrameCount = 0;
		}
		if (npcAudio != null)
		{
			npcAudio.volume = volume;
		}
	}

	protected override void loseHeart()
	{
		base.loseHeart();
	}

	protected override void respawn(float inSeconds, bool shouldLoseHeart)
	{
		base.respawn(inSeconds, shouldLoseHeart);
		resetWayPoint();
	}

	public void ApplyAISettings(KR_AISettings aiSettings)
	{
		settings = aiSettings;
		rubberBanding = settings.RubberBanding;
		baseDifficulty = settings.BaseDifficulty;
		playerName = aiSettings.LocalizationName;
		base.Spawner = GetComponentInChildren<ConfigKartSpawner>();
		((ConfigKartSpawner)base.Spawner).SetConfiguration(settings.Config);
	}

	private void resetWayPoint()
	{
		targetWayPoint = currentTrack.GetNearestWayPoint(base.gameObject.transform);
		prevTargetWayPoint = getPreviousTargetPoint(targetWayPoint);
		nextTargetWayPoint = getNextTargetPoint(targetWayPoint);
	}

	private void updateWayPoint()
	{
		Vector3 vector = base.transform.forward;
		Vector3 normalized = (targetWayPoint.Center.transform.position - base.transform.position).normalized;
		if (rigidBody != null && rigidBody.velocity.sqrMagnitude > 2f)
		{
			vector = rigidBody.velocity.normalized;
		}
		float num = Vector3.Angle(vector, normalized);
		if (num > 90f || Vector3.Distance(base.transform.position, targetWayPoint.GetClosestPoint(base.transform.position)) < 2.3f)
		{
			prevTargetWayPoint = targetWayPoint;
			targetWayPoint = nextTargetWayPoint;
			nextTargetWayPoint = getNextTargetPoint(nextTargetWayPoint);
		}
	}

	private KR_Waypoint getPreviousTargetPoint(KR_Waypoint waypoint)
	{
		KR_Waypoint kR_Waypoint = waypoint.Path.GetPreviousWayPoint(waypoint);
		if (kR_Waypoint == null)
		{
			KR_Route previousRoute = currentTrack.GetPreviousRoute(waypoint.Path.Route);
			kR_Waypoint = previousRoute.Paths.GetRandom().Waypoints.Last();
		}
		return kR_Waypoint;
	}

	private KR_Waypoint getNextTargetPoint(KR_Waypoint waypoint)
	{
		KR_Waypoint kR_Waypoint = waypoint.Path.GetNextWayPoint(waypoint);
		if (kR_Waypoint == null)
		{
			KR_Route nextRoute = currentTrack.GetNextRoute(waypoint.Path.Route);
			kR_Waypoint = nextRoute.Paths.GetRandom().Waypoints.First();
		}
		return kR_Waypoint;
	}

	private void updateDrivingStyle()
	{
		DrivingStyle drivingStyle = kartDrivingStyle;
		float num = Vector3.Distance(base.transform.position, player.transform.position);
		if (num > 10f)
		{
			if (RacePosition < player.RacePosition)
			{
				kartDrivingStyle = drivingStyles.FirstOrDefault((DrivingStyle s) => s.Type == DrivingStyleTypes.passive);
			}
			else
			{
				kartDrivingStyle = drivingStyles.FirstOrDefault((DrivingStyle s) => s.Type == DrivingStyleTypes.aggresive);
			}
		}
		else
		{
			kartDrivingStyle = drivingStyles.FirstOrDefault((DrivingStyle s) => s.Type == DrivingStyleTypes.normal);
		}
		if (drivingStyle != kartDrivingStyle)
		{
			applyKartSettings(kartSettings, baseDifficulty + rubberBanding * kartDrivingStyle.CheatFactor);
		}
	}

	private void updateSteer()
	{
		if (isDrivingTooClose)
		{
			float num = MathUtilities.AngleSigned(base.transform.forward, separationVector, Vector3.up);
			steer += ((num > 0f) ? 0.3f : ((!(num < 0f)) ? 0f : (-0.3f)));
		}
		else if (!isInFrontOfWayPoint)
		{
			steer += ((kartAngleToWPL > 0f) ? 0.6f : ((!(kartAngleToWPR < 0f)) ? 0f : (-0.6f)));
		}
		else if (!isAligned)
		{
			steer += ((kartAlignment > 5f) ? 0.3f : ((!(kartAlignment < -5f)) ? steer : (-0.3f)));
		}
		else if (steer != 0f)
		{
			steer = ((!(steer > -0.1f) && !(steer < 0.1f)) ? (steer / 2f) : 0f);
		}
		steer = Mathf.Round(steer * 100f) / 100f;
		steer = Mathf.Clamp(steer, -1f, 1f);
	}

	private void updateBrake()
	{
		if (isDrivingTooClose)
		{
			brake *= 2f;
		}
		else
		{
			brake = Mathf.Clamp(Mathf.Abs(kartAlignment), 0f, 20f) / 20f;
			brake = brakeCurve.Evaluate(brake);
			brake *= kartDrivingStyle.Brake;
		}
		brake = Mathf.Round(brake * 100f) / 100f;
		brake = Mathf.Clamp(brake, 0f, 1.2f);
		if (rigidBody.velocity.sqrMagnitude < 25f)
		{
			brake = 0f;
		}
	}

	private float getDistanceFromOpponent(KR_KartBase opponent)
	{
		return Vector3.Distance(base.transform.position, opponent.transform.position);
	}

	private void updateFlockingVectors()
	{
		int num = 0;
		alignmentVector = (nextTargetWayPoint.Center.position - targetWayPoint.Center.position).normalized;
		cohesionVector = Vector3.one;
		separationVector = default(Vector3);
		for (int i = 0; i < opponents.Length; i++)
		{
			float num2 = Vector3.Distance(base.transform.position, opponents[i].transform.position);
			if (num2 < 10f)
			{
				cohesionVector += opponents[i].transform.position;
				separationVector += (base.transform.position - opponents[i].transform.position).normalized / num2;
				num++;
			}
		}
		if (num > 0)
		{
			separationVector = (separationVector / num).normalized;
			cohesionVector = (cohesionVector / num - base.transform.position).normalized;
			separationVector = base.transform.position + separationVector - base.transform.position;
			cohesionVector = base.transform.position + cohesionVector - base.transform.position;
		}
	}

	private KR_KartBase[] getTooCloseOpponents()
	{
		return opponents.Where((KR_KartBase o) => getDistanceFromOpponent(o) <= 2.5f).ToArray();
	}
}

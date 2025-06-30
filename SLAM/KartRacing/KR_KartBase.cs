using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.KartRacing;

[RequireComponent(typeof(FiniteStateMachine))]
public class KR_KartBase : MonoBehaviour
{
	protected const string STATE_WAITING_TO_BEGIN = "Waiting to begin";

	protected const string STATE_RACING = "Racing";

	protected const string STATE_FINISHED = "Finished";

	public const int INITIAL_HEART_COUNT = 3;

	[SerializeField]
	protected Animator characterAnimator;

	[SerializeField]
	protected string playerName;

	[SerializeField]
	protected float impactForceToLoseHeart = 25f;

	[SerializeField]
	protected float invulnerabilityDuration = 3f;

	[SerializeField]
	protected float respawnDuration = 1f;

	[SerializeField]
	protected PrefabSpawner kartHitParticlesSpawner;

	[SerializeField]
	private PrefabSpawner[] wheelDustParticlesSpawner;

	[SerializeField]
	protected float highImpactThreshold = 20f;

	[SerializeField]
	protected float mediumImpactThreshold = 13f;

	[SerializeField]
	protected float lowImpactThreshold = 6f;

	[SerializeField]
	private AnimationCurve driveToFinishPositionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	private float driveToFinishPositionDuration = 1f;

	protected float steer;

	protected float brake;

	protected FiniteStateMachine fsm;

	private Alarm timer;

	protected KR_Track currentTrack;

	protected bool isInvulnerable;

	private int heartsLeft = 3;

	protected bool isRespawning;

	protected KR_Waypoint nearestWayPoint;

	public Alarm Timer => timer;

	public string PlayerName => playerName;

	public bool HasFinished { get; protected set; }

	public virtual int HeartsLeft
	{
		get
		{
			return heartsLeft;
		}
		protected set
		{
			heartsLeft = value;
		}
	}

	public float TrackProgress { get; private set; }

	public virtual int RacePosition { get; set; }

	protected virtual void Awake()
	{
	}

	protected virtual void Start()
	{
		fsm = GetComponent<FiniteStateMachine>();
		fsm.AddState("Waiting to begin", OnEnterWaitingToBegin, null, null);
		fsm.AddState("Racing", OnEnterRacing, WhileRacing, null);
		fsm.AddState("Finished", OnEnterFinished, null, null);
		fsm.SwitchTo("Waiting to begin");
	}

	protected virtual void FixedUpdate()
	{
	}

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<KR_StartRaceEvent>(onStartRace);
		GameEvents.Subscribe<KR_FinishCrossedEvent>(onFinishCrossed);
		GameEvents.Subscribe<KR_PickupEvent>(onPickUp);
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onStartRace);
		GameEvents.Unsubscribe<KR_FinishCrossedEvent>(onFinishCrossed);
		GameEvents.Unsubscribe<KR_PickupEvent>(onPickUp);
	}

	protected virtual void OnEnterWaitingToBegin()
	{
		timer = Alarm.Create(base.transform);
		characterAnimator.SetTrigger("Activate");
	}

	protected virtual void OnEnterRacing()
	{
		timer.StartCountUp();
		nearestWayPoint = currentTrack.GetNearestWayPoint(base.transform);
	}

	protected virtual void WhileRacing()
	{
		characterAnimator.SetFloat("Steer", steer);
		nearestWayPoint = currentTrack.GetNearestWayPoint(base.transform, nearestWayPoint);
		TrackProgress = currentTrack.GetProgress(base.transform, nearestWayPoint);
	}

	protected virtual void OnEnterFinished()
	{
		HasFinished = true;
		timer.Pause();
	}

	public void DriveToLocation(Transform location)
	{
		StartCoroutine(driveToFinishPositionRoutine(location));
	}

	protected virtual void onStartRace(KR_StartRaceEvent evt)
	{
		currentTrack = evt.Track;
		fsm.SwitchTo("Racing");
	}

	protected virtual void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		if (evt.Kart == this)
		{
			if (evt.PodiumPosition == 1)
			{
				characterAnimator.SetTrigger("Cheer");
			}
			else
			{
				characterAnimator.SetTrigger("Fail");
			}
			fsm.SwitchTo("Finished");
		}
	}

	protected virtual void onPickUp(KR_PickupEvent evt)
	{
	}

	protected virtual void loseHeart()
	{
		HeartsLeft = Mathf.Clamp(HeartsLeft - 1, 0, 3);
	}

	private IEnumerator driveToFinishPositionRoutine(Transform target)
	{
		Vector3 startPos = base.transform.position;
		Vector3 endPos = target.position;
		Quaternion startRot = base.transform.rotation;
		Quaternion endRot = target.rotation;
		yield return new WaitForEndOfFrame();
		float time = 0f;
		while (time < driveToFinishPositionDuration)
		{
			time += Time.deltaTime;
			base.transform.position = Vector3.Lerp(startPos, endPos, driveToFinishPositionCurve.Evaluate(time / driveToFinishPositionDuration));
			base.transform.rotation = Quaternion.Slerp(startRot, endRot, driveToFinishPositionCurve.Evaluate(time / driveToFinishPositionDuration));
			yield return null;
		}
	}
}

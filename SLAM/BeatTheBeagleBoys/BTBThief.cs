using System.Collections;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBThief : BTBCharacter
{
	private const int ANIM_IDLE = 0;

	private const int ANIM_SNEAK = 1;

	private const int ANIM_RUN = 2;

	private const int ANIM_RUN_AWAY = 3;

	private const int ANIM_RUN_AWAY_WITH_ANIMAL = 4;

	private const int ANIM_STEAL = 5;

	private const int ANIM_SNEAK_LOOK = 6;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float sneak_speed = 1.5f;

	[SerializeField]
	[Range(0.1f, 10f)]
	private float run_speed = 3.5f;

	[SerializeField]
	private Transform burlapSackLocator;

	[SerializeField]
	private GameObject burlapSack;

	private Animator thiefAnim;

	private bool hasStolen;

	private bool routeForward;

	private BTBArea.RoutePoints route;

	public bool IsIdling { get; private set; }

	public bool IsRunningToCage { get; private set; }

	public bool IsStealing { get; private set; }

	public bool IsFleeing { get; private set; }

	public override void Initialize(BTBArea area, BTBDifficultySetting difficulty)
	{
		base.Initialize(area, difficulty);
		route = area.Routes.GetRandom();
		routeForward = Random.value >= 0.5f;
		base.transform.position = ((!routeForward) ? route.exitPoint.position : route.entryPoint.position);
		navAgent.enabled = true;
		thiefAnim = GetComponent<Animator>();
		StartCoroutine(doSequence());
	}

	public void stopAI()
	{
		StopAllCoroutines();
		StandStill();
	}

	private IEnumerator doSequence()
	{
		IsIdling = true;
		yield return StartCoroutine(doIdle());
		if ((double)Random.value > 0.5)
		{
			yield return StartCoroutine(doSneakLook());
			yield return StartCoroutine(doIdle());
		}
		IsIdling = false;
		if (base.currentArea.IsMonitored)
		{
			IsRunningToCage = true;
			yield return StartCoroutine(doMoveToStealPoint());
			IsRunningToCage = false;
			if (base.currentArea.CanSteal)
			{
				IsStealing = true;
				yield return StartCoroutine(doSteal());
				IsStealing = false;
			}
		}
		IsFleeing = true;
		yield return StartCoroutine(doMoveToExitPoint());
	}

	private IEnumerator doIdle()
	{
		Stopwatch idleTimer = new Stopwatch(base.settings.GetThiefIdlingTime());
		Sneak();
		moveToPosition(route.stealPoint.position);
		while (!idleTimer.Expired && !base.currentArea.CanSteal)
		{
			yield return null;
		}
	}

	private IEnumerator doSneakLook()
	{
		Stopwatch waitTimer = new Stopwatch(1.5f);
		SneakLook();
		while (!waitTimer.Expired)
		{
			yield return null;
		}
	}

	private IEnumerator doMoveToStealPoint()
	{
		if (!base.currentArea.CanSteal)
		{
			base.currentArea.Cage.OnHacked();
		}
		Run();
		moveToPosition(route.stealPoint.position);
		while (!base.PositionReached && base.currentArea.CanSteal)
		{
			yield return null;
		}
	}

	private IEnumerator doSteal()
	{
		hasStolen = true;
		moveToPosition(route.animalPoint.position);
		while (!base.PositionReached)
		{
			yield return null;
		}
		Steal();
		Stopwatch stealTimer = new Stopwatch(base.settings.GetThiefStealingTime());
		base.currentArea.Cage.OnSteal();
		while (!stealTimer.Expired)
		{
			yield return null;
		}
	}

	private IEnumerator doMoveToExitPoint()
	{
		if (hasStolen)
		{
			attachBurlapSack();
			RunAwayWithAnimal();
		}
		else
		{
			RunAway();
		}
		moveToPosition((!routeForward) ? route.entryPoint.position : route.exitPoint.position);
		while (!base.PositionReached)
		{
			yield return null;
		}
		base.currentArea.OnThiefExited(this);
		base.enabled = false;
	}

	private void attachBurlapSack()
	{
		GameObject gameObject = Object.Instantiate(burlapSack);
		gameObject.transform.position = base.transform.position;
		gameObject.transform.rotation = base.transform.rotation;
		gameObject.transform.parent = burlapSackLocator;
	}

	private void StandStill()
	{
		thiefAnim.SetInteger("animType", 0);
		navAgent.speed = 0f;
	}

	private void Sneak()
	{
		thiefAnim.SetInteger("animType", 1);
		navAgent.speed = sneak_speed;
	}

	private void SneakLook()
	{
		thiefAnim.SetInteger("animType", 6);
		navAgent.speed = 0f;
	}

	private void Steal()
	{
		thiefAnim.SetInteger("animType", 5);
		navAgent.speed = 0f;
	}

	private void Run()
	{
		thiefAnim.SetInteger("animType", 2);
		navAgent.speed = run_speed;
	}

	private void RunAway()
	{
		thiefAnim.SetInteger("animType", 3);
		navAgent.speed = run_speed;
	}

	private void RunAwayWithAnimal()
	{
		thiefAnim.SetInteger("animType", 4);
		navAgent.speed = run_speed;
	}

	private void OnFootstep()
	{
	}
}

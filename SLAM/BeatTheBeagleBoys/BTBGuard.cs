using System.Collections;
using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBGuard : BTBCharacter
{
	private const int ANIM_IDLE = 0;

	private const int ANIM_WALK = 1;

	[SerializeField]
	private float walk_speed = 2f;

	private Animator guardAnim;

	private BTBArea.RoutePoints route;

	public override void Initialize(BTBArea area, BTBDifficultySetting difficulty)
	{
		base.Initialize(area, difficulty);
		route = area.Routes.GetRandom();
		base.transform.position = route.entryPoint.position;
		navAgent.enabled = true;
		guardAnim = GetComponent<Animator>();
		StartCoroutine(doSequence());
	}

	private IEnumerator doSequence()
	{
		yield return StartCoroutine(doMoveToExitPoint());
	}

	private IEnumerator doMoveToExitPoint()
	{
		Walk();
		moveToPosition(route.exitPoint.position);
		while (!base.PositionReached)
		{
			yield return null;
		}
		base.currentArea.OnGuardExited(this);
		base.enabled = false;
	}

	private void StandStill()
	{
		guardAnim.SetInteger("animType", 0);
		navAgent.speed = 0f;
	}

	private void Walk()
	{
		guardAnim.SetInteger("animType", 1);
		navAgent.speed = walk_speed;
	}
}

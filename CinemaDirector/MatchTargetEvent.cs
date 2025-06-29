using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Match Target", new CutsceneItemGenre[] { CutsceneItemGenre.MecanimItem })]
public class MatchTargetEvent : CinemaActorEvent
{
	public GameObject target;

	public AvatarTarget avatarTarget;

	public float startTime;

	public float targetTime;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.MatchTarget(weightMask: new MatchTargetWeightMask(Vector3.one, 0f), matchPosition: target.transform.position, matchRotation: target.transform.rotation, targetBodyPart: avatarTarget, startNormalizedTime: startTime, targetNormalizedTime: targetTime);
		}
	}
}

using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Target", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetTargetAnimatorEvent : CinemaActorEvent
{
	public AvatarTarget TargetIndex;

	public float TargetNormalizedTime;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetTarget(TargetIndex, TargetNormalizedTime);
		}
	}
}

using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set IK Position Weight", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetIKPositionWeightAnimatorEvent : CinemaActorEvent
{
	public AvatarIKGoal Goal;

	public float Value;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetIKPositionWeight(Goal, Value);
		}
	}
}

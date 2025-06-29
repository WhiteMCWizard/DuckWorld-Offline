using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set IK Rotation Weight", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetIKRotationWeightAnimatorEvent : CinemaActorEvent
{
	public AvatarIKGoal Goal;

	public float Value;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetIKRotationWeight(Goal, Value);
		}
	}
}

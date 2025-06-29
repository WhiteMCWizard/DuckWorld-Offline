using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set IK Rotation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetIKRotationAnimatorEvent : CinemaActorEvent
{
	public AvatarIKGoal Goal;

	public Quaternion GoalRotation;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetIKRotation(Goal, GoalRotation);
		}
	}
}

using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set IK Position", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetIKPositionAnimatorEvent : CinemaActorEvent
{
	public AvatarIKGoal Goal;

	public Vector3 GoalPosition;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetIKPosition(Goal, GoalPosition);
		}
	}
}

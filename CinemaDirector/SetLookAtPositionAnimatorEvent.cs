using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Look At Position", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetLookAtPositionAnimatorEvent : CinemaActorEvent
{
	public Transform LookAtPosition;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetLookAtPosition(LookAtPosition.position);
		}
	}
}

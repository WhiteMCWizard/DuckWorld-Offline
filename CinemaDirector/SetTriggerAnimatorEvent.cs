using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Trigger", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetTriggerAnimatorEvent : CinemaActorEvent
{
	public string Name;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetTrigger(Name);
		}
	}
}

using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Integer", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetIntegerAnimatorEvent : CinemaActorEvent
{
	public string IntName;

	public int Value;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetInteger(IntName, Value);
		}
	}
}

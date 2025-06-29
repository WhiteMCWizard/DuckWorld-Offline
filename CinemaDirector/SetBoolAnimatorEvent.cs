using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Bool", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetBoolAnimatorEvent : CinemaActorEvent
{
	public string BoolName;

	public bool Value;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (component != null)
		{
			component.SetBool(BoolName, Value);
		}
	}
}

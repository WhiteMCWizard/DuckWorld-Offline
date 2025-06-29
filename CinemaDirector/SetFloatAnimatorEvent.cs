using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animator", "Set Float", new CutsceneItemGenre[]
{
	CutsceneItemGenre.ActorItem,
	CutsceneItemGenre.MecanimItem
})]
public class SetFloatAnimatorEvent : CinemaActorEvent
{
	public string FloatName;

	public float Value;

	public override void Trigger(GameObject actor)
	{
		Animator component = actor.GetComponent<Animator>();
		if (!(component == null))
		{
			component.SetFloat(FloatName, Value);
		}
	}
}

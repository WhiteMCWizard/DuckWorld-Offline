using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Physics", "Wake Up", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class RigidbodyWakeUpEvent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
		Rigidbody component = actor.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.WakeUp();
		}
	}

	public override void Reverse(GameObject actor)
	{
		Rigidbody component = actor.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.Sleep();
		}
	}
}

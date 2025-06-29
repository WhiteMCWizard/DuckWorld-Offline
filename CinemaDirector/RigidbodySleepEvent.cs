using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Physics", "Sleep", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class RigidbodySleepEvent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Rigidbody component = actor.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.Sleep();
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
		if (actor != null)
		{
			Rigidbody component = actor.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.WakeUp();
			}
		}
	}
}

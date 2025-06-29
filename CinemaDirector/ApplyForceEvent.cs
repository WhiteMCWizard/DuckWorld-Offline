using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Physics", "Apply Force", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ApplyForceEvent : CinemaActorEvent
{
	public Vector3 Force = Vector3.forward;

	public ForceMode ForceMode = ForceMode.Impulse;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Rigidbody component = actor.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.AddForce(Force, ForceMode);
			}
		}
	}
}

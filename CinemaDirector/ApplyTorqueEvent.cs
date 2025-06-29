using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Physics", "Apply Torque", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ApplyTorqueEvent : CinemaActorEvent
{
	public Vector3 Torque = Vector3.forward;

	public ForceMode ForceMode = ForceMode.Impulse;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Rigidbody component = actor.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.AddTorque(Torque, ForceMode);
			}
		}
	}
}

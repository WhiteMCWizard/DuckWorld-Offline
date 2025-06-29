using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transform", "Set Position", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetPositionEvent : CinemaActorEvent
{
	public Vector3 Position;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			actor.transform.position = Position;
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

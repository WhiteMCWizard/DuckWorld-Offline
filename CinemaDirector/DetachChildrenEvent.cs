using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transform", "Detach Children", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class DetachChildrenEvent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			actor.transform.DetachChildren();
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

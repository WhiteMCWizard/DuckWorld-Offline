using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transform", "Set Parent", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetParent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
	}

	public override void Reverse(GameObject actor)
	{
	}
}

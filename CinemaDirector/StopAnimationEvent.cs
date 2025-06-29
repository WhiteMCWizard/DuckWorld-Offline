using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Stop Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class StopAnimationEvent : CinemaActorEvent
{
	public string Animation = string.Empty;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Animation component = actor.GetComponent<Animation>();
			if ((bool)component)
			{
				component.Stop(Animation);
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

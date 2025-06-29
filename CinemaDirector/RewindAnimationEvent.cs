using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Rewind Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class RewindAnimationEvent : CinemaActorEvent
{
	public string Animation = string.Empty;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Animation component = actor.GetComponent<Animation>();
			if ((bool)component)
			{
				component.Rewind(Animation);
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Cross Fade Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class CrossFadeAnimationEvent : CinemaActorEvent
{
	public string Animation = string.Empty;

	public float TargetWeight = 1f;

	public PlayMode PlayMode;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Animation component = actor.GetComponent<Animation>();
			if ((bool)component)
			{
				component.CrossFade(Animation, TargetWeight, PlayMode);
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

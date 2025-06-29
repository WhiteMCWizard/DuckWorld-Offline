using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Blend Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class BlendAnimationEvent : CinemaActorEvent
{
	public string Animation = string.Empty;

	public float TargetWeight = 1f;

	public float FadeLength = 0.3f;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Animation component = actor.GetComponent<Animation>();
			if ((bool)component)
			{
				component.Blend(Animation, TargetWeight, FadeLength);
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

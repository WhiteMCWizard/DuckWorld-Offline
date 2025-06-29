using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Animation", "Sample Animation", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SampleAnimationEvent : CinemaActorEvent
{
	public string Animation = string.Empty;

	public float Time;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Animation component = actor.GetComponent<Animation>();
			if ((bool)component)
			{
				component[Animation].time = Time;
				component[Animation].enabled = true;
				component.Sample();
				component[Animation].enabled = false;
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}

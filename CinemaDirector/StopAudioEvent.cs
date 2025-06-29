using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio Source", "Stop Audio", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class StopAudioEvent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			AudioSource component = actor.GetComponent<AudioSource>();
			if ((bool)component)
			{
				component.Stop();
			}
		}
	}
}

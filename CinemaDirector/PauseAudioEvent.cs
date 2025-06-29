using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio Source", "Pause Audio", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class PauseAudioEvent : CinemaActorEvent
{
	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			AudioSource component = actor.GetComponent<AudioSource>();
			if ((bool)component)
			{
				component.Pause();
			}
		}
	}
}

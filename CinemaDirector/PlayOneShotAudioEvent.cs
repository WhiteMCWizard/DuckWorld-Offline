using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio Source", "Play One Shot", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class PlayOneShotAudioEvent : CinemaActorEvent
{
	public AudioClip Clip;

	public float VolumeScale = 1f;

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			AudioSource component = actor.GetComponent<AudioSource>();
			if ((bool)component)
			{
				component.PlayOneShot(Clip, VolumeScale);
			}
		}
	}
}

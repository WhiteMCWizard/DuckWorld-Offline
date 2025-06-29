using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio Source", "Play Clip At Point", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class PlayClipAtPointAudioEvent : CinemaGlobalEvent
{
	public AudioClip Clip;

	public Vector3 Position;

	public float VolumeScale = 1f;

	public override void Trigger()
	{
		AudioSource.PlayClipAtPoint(Clip, Position, VolumeScale);
	}
}

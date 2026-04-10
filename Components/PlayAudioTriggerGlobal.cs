using CinemaDirector;
using UnityEngine;

[CutsceneItem("SLAM", "Play Audio Trigger", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class PlayAudioTriggerGlobal : CinemaGlobalEvent
{
	[SerializeField]
	private AudioClip clip;

	public override void Trigger()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && clip != null)
		{
			AudioController.Play(clip.name);
		}
	}
}

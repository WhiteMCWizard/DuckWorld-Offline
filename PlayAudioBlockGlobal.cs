using CinemaDirector;
using UnityEngine;

[CutsceneItem("SLAM", "Play Audio Block", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class PlayAudioBlockGlobal : CinemaGlobalAction
{
	[SerializeField]
	private AudioClip clip;

	[SerializeField]
	private bool loop;

	private AudioObject audioObj;

	public override void Trigger()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && clip != null)
		{
			if (loop)
			{
				AudioController.GetAudioItem(clip.name).Loop = AudioItem.LoopMode.LoopSubitem;
			}
			audioObj = AudioController.Play(clip.name);
		}
	}

	public override void End()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && audioObj != null)
		{
			AudioController.Stop(audioObj.audioID);
		}
	}
}

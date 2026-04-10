using UnityEngine;

public class PlayAudioOnAnimationEvent : MonoBehaviour
{
	private void PlayAudio(AudioClip clip)
	{
		AudioController.Play(clip.name);
	}

	private void PlayAudioWorld(AudioClip clip)
	{
		AudioController.Play(clip.name, base.transform);
	}

	private void StopAudio(AudioClip clip)
	{
		AudioController.Stop(clip.name);
	}
}

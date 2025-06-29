using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio Source", "Play Audio", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class PlayAudioEvent : CinemaActorAction
{
	public AudioClip audioClip;

	public bool loop;

	private bool wasPlaying;

	public void Update()
	{
		if (!loop && (bool)audioClip)
		{
			base.Duration = audioClip.length;
		}
		else
		{
			base.Duration = -1f;
		}
	}

	public override void Trigger(GameObject Actor)
	{
		AudioSource audioSource = Actor.GetComponent<AudioSource>();
		if (!audioSource)
		{
			audioSource = Actor.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		if (audioSource.clip != audioClip)
		{
			audioSource.clip = audioClip;
		}
		audioSource.time = 0f;
		audioSource.loop = loop;
		audioSource.Play();
	}

	public override void UpdateTime(GameObject Actor, float runningTime, float deltaTime)
	{
		AudioSource audioSource = Actor.GetComponent<AudioSource>();
		if (!audioSource)
		{
			audioSource = Actor.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		if (audioSource.clip != audioClip)
		{
			audioSource.clip = audioClip;
		}
		if (!audioSource.isPlaying)
		{
			audioSource.time = deltaTime;
			audioSource.Play();
		}
	}

	public override void Resume(GameObject Actor)
	{
		AudioSource component = Actor.GetComponent<AudioSource>();
		if ((bool)component)
		{
			component.time = base.Cutscene.RunningTime - base.Firetime;
			if (wasPlaying)
			{
				component.Play();
			}
		}
	}

	public override void Pause(GameObject Actor)
	{
		AudioSource component = Actor.GetComponent<AudioSource>();
		wasPlaying = false;
		if ((bool)component && component.isPlaying)
		{
			wasPlaying = true;
		}
		if ((bool)component)
		{
			component.Pause();
		}
	}

	public override void End(GameObject Actor)
	{
		AudioSource component = Actor.GetComponent<AudioSource>();
		if ((bool)component)
		{
			component.Stop();
		}
	}
}

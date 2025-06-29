using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Audio", "Play Audio", typeof(AudioClip), new CutsceneItemGenre[] { CutsceneItemGenre.AudioClipItem })]
public class CinemaAudio : TimelineActionFixed
{
	private bool wasPlaying;

	public void Trigger()
	{
	}

	public void End()
	{
		Stop();
	}

	public void UpdateTime(float time, float deltaTime)
	{
		AudioSource component = base.gameObject.GetComponent<AudioSource>();
		if (!(component != null) || !(component.clip != null))
		{
			return;
		}
		component.mute = false;
		time = Mathf.Clamp(time, 0f, component.clip.length) + base.InTime;
		if ((double)(component.clip.length - time) > 0.0001)
		{
			if (base.Cutscene.State == Cutscene.CutsceneState.Scrubbing)
			{
				component.time = time;
			}
			if (!component.isPlaying)
			{
				component.time = time;
				component.Play();
			}
		}
	}

	public void Resume()
	{
		AudioSource component = base.gameObject.GetComponent<AudioSource>();
		if (component != null && wasPlaying)
		{
			component.Play();
		}
	}

	public void Pause()
	{
		AudioSource component = base.gameObject.GetComponent<AudioSource>();
		if (component != null)
		{
			wasPlaying = false;
			if (component.isPlaying)
			{
				wasPlaying = true;
			}
			component.Pause();
		}
	}

	public override void Stop()
	{
		AudioSource component = base.gameObject.GetComponent<AudioSource>();
		if ((bool)component)
		{
			component.Stop();
		}
	}

	public void SetTime(float audioTime)
	{
		AudioSource component = base.gameObject.GetComponent<AudioSource>();
		if (component != null && component.clip != null)
		{
			audioTime = Mathf.Clamp(audioTime, 0f, component.clip.length);
			if ((double)(component.clip.length - audioTime) > 0.0001)
			{
				component.time = audioTime;
			}
		}
	}

	public override void SetDefaults(Object PairedItem)
	{
		AudioClip audioClip = PairedItem as AudioClip;
		if (audioClip != null)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.clip = audioClip;
			base.Firetime = 0f;
			base.Duration = audioClip.length;
			base.InTime = 0f;
			base.OutTime = audioClip.length;
			base.ItemLength = audioClip.length;
			audioSource.playOnAwake = false;
		}
	}
}

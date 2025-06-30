using System.Collections;
using UnityEngine;

namespace SLAM.Shared;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
	private const string MUSIC_VOLUME_KEY = "MusicVolume";

	private const string SFX_VOLUME_KEY = "SFXVolume";

	private const string MUSIC_CATEGORY = "Music";

	private const string SFX_CATEGORY = "SFX";

	private const string AMBIENCE_CATEGORY = "Ambience";

	private float globalVolume = 1f;

	private float musicVolume = 1f;

	private float sfxVolume = 1f;

	public float GlobalVolume
	{
		get
		{
			return globalVolume;
		}
		set
		{
			globalVolume = value;
			AudioController.SetGlobalVolume(globalVolume);
		}
	}

	public float MusicVolume
	{
		get
		{
			return musicVolume;
		}
		set
		{
			musicVolume = value;
			AudioController.SetCategoryVolume("Music", musicVolume);
			PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		}
	}

	public float SfxVolume
	{
		get
		{
			return sfxVolume;
		}
		set
		{
			sfxVolume = value;
			if (AudioController.GetCategory("SFX") != null)
			{
				AudioController.SetCategoryVolume("SFX", sfxVolume);
			}
			if (AudioController.GetCategory("Ambience") != null)
			{
				AudioController.SetCategoryVolume("Ambience", sfxVolume);
			}
			PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
		}
	}

	private void OnLevelWasLoaded(int level)
	{
		GlobalVolume = 1f;
		MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
		SfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
	}

	public void FadeOut()
	{
		StartCoroutine(doFadeOut());
	}

	private IEnumerator doFadeOut()
	{
		for (float t = GlobalVolume; t > 0f; t -= Time.deltaTime / 0.5f)
		{
			GlobalVolume = t;
			yield return null;
		}
		GlobalVolume = 0f;
	}
}

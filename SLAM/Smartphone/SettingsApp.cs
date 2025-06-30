using System;
using SLAM.BuildSystem;
using SLAM.Performance;
using SLAM.Shared;
using UnityEngine;

namespace SLAM.Smartphone;

public class SettingsApp : AppController
{
	private Quality graphicsQuality;

	public Quality GraphicsQuality
	{
		get
		{
			return graphicsQuality;
		}
		set
		{
			graphicsQuality = value;
		}
	}

	public override void Open()
	{
		// enable language dropdown
		var languageDropdown = GameObject.Find("Smartphone3D/3D UI Camera/Smartphone/Parent/UI Root/SettingsApp/SettingsView/Anchor Center/Scroll View/Grid/LanguageDropdown");
		if (languageDropdown != null)
		{
			languageDropdown.SetActive(true);
		}
		
		string version = "v" + SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString();
		OpenView<SettingsView>().SetData(SingletonMonobehaviour<AudioManager>.Instance.MusicVolume, SingletonMonobehaviour<AudioManager>.Instance.SfxVolume, SingletonMonobehaviour<PerformanceManager>.Instance.CurrentQuality, version);
	}

	public void SetMusicVolume(float volume)
	{
		SingletonMonobehaviour<AudioManager>.Instance.MusicVolume = Mathf.Clamp01(volume);
	}

	public void SetSfxVolume(float volume)
	{
		SingletonMonobehaviour<AudioManager>.Instance.SfxVolume = Mathf.Clamp01(volume);
	}

	public void SetQuality(Quality quality)
	{
		SingletonMonobehaviour<PerformanceManager>.Instance.CurrentQuality = quality;
	}

	protected override void checkForNotifications(Action<AppChangedEvent> callback)
	{
	}
}

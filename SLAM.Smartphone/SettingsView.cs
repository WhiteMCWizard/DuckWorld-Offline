using System;
using SLAM.Performance;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Smartphone;

public class SettingsView : AppView
{
	private const string SETTING_FULLSCREEN = "SETTINGS_FULLSCREEN";

	private const string SETTING_LANGUAGE = "SETTINGS_LANGUAGE";

	private const string SETTING_RESOLUTION_WIDTH = "SETTING_RESOLUTION_WIDTH";

	private const string SETTING_RESOLUTION_HEIGHT = "SETTING_RESOLUTION_HEIGHT";

	private const string SETTING_RESOLUTION_REFRESHRATE = "SETTING_RESOLUTION_REFRESHRATE";

	[SerializeField]
	private UIButton prevButton;

	[SerializeField]
	private UIButton nextButton;

	[SerializeField]
	private UISlider musicSlider;

	[SerializeField]
	private UISlider sfxSlider;

	[SerializeField]
	private UILabel qualityLabel;

	[SerializeField]
	private UILabel versionLabel;

	[SerializeField]
	private UIToggle fullscreenToggle;

	[SerializeField]
	private UIPopupList languagesPopupList;

	[SerializeField]
	private UIPopupList resolutionPopupList;

	private Quality quality = Quality.Medium;

	public Resolution CurrentGameResolution => new Resolution
	{
		width = Screen.width,
		height = Screen.height,
		refreshRate = Screen.currentResolution.refreshRate
	};

	public static void InitializeSettings()
	{
		Screen.SetResolution(PlayerPrefs.GetInt("SETTING_RESOLUTION_WIDTH", Screen.width), PlayerPrefs.GetInt("SETTING_RESOLUTION_HEIGHT", Screen.height), PlayerPrefs.GetInt("SETTINGS_FULLSCREEN", 0) == 0);
		Localization.language = PlayerPrefs.GetString("SETTINGS_LANGUAGE", "Dutch");
	}

	private void OnEnable()
	{
		Localization.onLocalize = (Localization.OnLocalizeNotification)Delegate.Combine(Localization.onLocalize, new Localization.OnLocalizeNotification(setLanguages));
	}

	private void OnDisable()
	{
		Localization.onLocalize = (Localization.OnLocalizeNotification)Delegate.Remove(Localization.onLocalize, new Localization.OnLocalizeNotification(setLanguages));
	}

	public void SetData(float musicVolume, float sfxVolume, Quality quality, string version)
	{
		musicSlider.value = musicVolume;
		sfxSlider.value = sfxVolume;
		this.quality = quality;
		versionLabel.text = version;
		fullscreenToggle.value = Screen.fullScreen;
		setLanguages();
		setResolutions();
		setQualityLabel(quality);
	}

	private void setLanguages()
	{
		if (Localization.knownLanguages.Length > 0)
		{
			languagesPopupList.items.Clear();
			for (int i = 0; i < Localization.knownLanguages.Length; i++)
			{
				languagesPopupList.AddItem(Localization.Get("LANGUAGE_" + Localization.knownLanguages[i].ToUpper()), Localization.knownLanguages[i]);
			}
			int num = languagesPopupList.itemData.IndexOf(Localization.language);
			if (num != -1)
			{
				languagesPopupList.value = languagesPopupList.items[num];
			}
		}
	}

	private void setResolutions()
	{
		resolutionPopupList.items.Clear();
		resolutionPopupList.items.AddRange(Screen.resolutions.Select((Resolution r) => formatResolution(r)));
		if (!resolutionPopupList.itemData.Contains(formatResolution(Screen.currentResolution)))
		{
			resolutionPopupList.itemData.Add(formatResolution(Screen.currentResolution));
		}
		resolutionPopupList.value = formatResolution(CurrentGameResolution);
	}

	private string formatResolution(Resolution r)
	{
		return $"{r.width} x {r.height}";
	}

	public void OnLanguageChanged()
	{
		int num = languagesPopupList.items.IndexOf(languagesPopupList.value);
		if (num != -1)
		{
			Localization.language = languagesPopupList.itemData[num].ToString();
			PlayerPrefs.SetString("SETTINGS_LANGUAGE", Localization.language);
		}
	}

	public void OnFullscreenChanged()
	{
		if (base.IsOpen && fullscreenToggle.value != Screen.fullScreen)
		{
			UIToggle uIToggle = fullscreenToggle;
			bool value = (Screen.fullScreen = !Screen.fullScreen);
			uIToggle.value = value;
			PlayerPrefs.SetInt("SETTINGS_FULLSCREEN", (!fullscreenToggle.value) ? 1 : 0);
		}
	}

	public void OnResolutionChanged()
	{
		Resolution a = Screen.resolutions.FirstOrDefault((Resolution r) => formatResolution(r) == resolutionPopupList.value);
		if (!compareScreenResolution(a, CurrentGameResolution) && a.width > 0 && a.height > 0)
		{
			Screen.SetResolution(a.width, a.height, Screen.fullScreen);
			PlayerPrefs.SetInt("SETTING_RESOLUTION_WIDTH", a.width);
			PlayerPrefs.SetInt("SETTING_RESOLUTION_HEIGHT", a.height);
		}
	}

	private bool compareScreenResolution(Resolution a, Resolution b)
	{
		return a.width == b.width && a.height == b.height;
	}

	public void OnMusicVolumeChanged()
	{
		Controller<SettingsApp>().SetMusicVolume(musicSlider.value);
	}

	public void OnSfxVolumeChanged()
	{
		Controller<SettingsApp>().SetSfxVolume(sfxSlider.value);
	}

	public void OnPreviousButtonClicked()
	{
		switch (quality)
		{
		case Quality.High:
			quality = Quality.Medium;
			break;
		case Quality.Medium:
			quality = Quality.Low;
			break;
		}
		setQualityLabel(quality);
		Controller<SettingsApp>().SetQuality(quality);
	}

	public void OnNextButtonClicked()
	{
		switch (quality)
		{
		case Quality.Medium:
			quality = Quality.High;
			break;
		case Quality.Low:
			quality = Quality.Medium;
			break;
		}
		setQualityLabel(quality);
		Controller<SettingsApp>().SetQuality(quality);
	}

	private void setQualityLabel(Quality q)
	{
		prevButton.isEnabled = q != Quality.Low;
		nextButton.isEnabled = q != Quality.High;
		string empty = string.Empty;
		empty = q switch
		{
			Quality.High => Localization.Get("SF_SETTINGS_QUALITY_HIGH"), 
			Quality.Low => Localization.Get("SF_SETTINGS_QUALITY_LOW"), 
			_ => Localization.Get("SF_SETTINGS_QUALITY_MEDIUM"), 
		};
		qualityLabel.text = empty;
	}
}

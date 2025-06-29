using UnityEngine;

namespace SLAM.Performance;

public class PerformanceManager : SingletonMonobehaviour<PerformanceManager>
{
	public const string QUALITY_KEY = "CurrentQuality";

	private Quality currentQuality = Quality.Low;

	public Quality CurrentQuality
	{
		get
		{
			return currentQuality;
		}
		set
		{
			currentQuality = value;
			PlayerPrefs.SetInt("CurrentQuality", (int)currentQuality);
			PerformanceProfileChangedEvent performanceProfileChangedEvent = new PerformanceProfileChangedEvent();
			performanceProfileChangedEvent.NewQuality = CurrentQuality;
			GameEvents.Invoke(performanceProfileChangedEvent);
		}
	}

	private void Start()
	{
		if (!PlayerPrefs.HasKey("CurrentQuality"))
		{
			PlayerPrefs.SetInt("CurrentQuality", (int)guessQuality());
		}
		CurrentQuality = (Quality)PlayerPrefs.GetInt("CurrentQuality");
		Debug.Log("Current quality profile: " + CurrentQuality);
	}

	private Quality guessQuality()
	{
		Quality quality = Quality.Low;
		string text = SystemInfo.graphicsDeviceVendor.ToLower();
		Debug.Log("Your GPU vendor is: " + text + " " + SystemInfo.graphicsDeviceName + " and supports ShaderModel " + SystemInfo.graphicsShaderLevel);
		if (text.Contains("intel") && SystemInfo.graphicsShaderLevel < 50)
		{
			quality = Quality.Low;
		}
		else if (text.Contains("amd") || text.Contains("nvidia"))
		{
			if (SystemInfo.graphicsShaderLevel < 30)
			{
				quality = Quality.Low;
			}
			else if (SystemInfo.graphicsShaderLevel < 50)
			{
				quality = Quality.Medium;
			}
			else
			{
				quality = Quality.High;
			}
		}
		else
		{
			quality = Quality.Low;
		}
		return Quality.High;
	}
}

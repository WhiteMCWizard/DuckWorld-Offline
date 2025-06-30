using System.Collections;
using SLAM.Engine;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSStatsView : View
{
	[SerializeField]
	private UIProgressBar topSpeedProgressbar;

	[SerializeField]
	private UIProgressBar accelerationProgressbar;

	[SerializeField]
	private UIProgressBar handlingProgressbar;

	private float maxStatValue = 1f;

	public void SetInfo(KartConfigurationData kartConfig)
	{
		StartCoroutine(animateProgress(topSpeedProgressbar, kartConfig.GetStat(KartSystem.ItemStat.TopSpeed) / maxStatValue));
		StartCoroutine(animateProgress(accelerationProgressbar, kartConfig.GetStat(KartSystem.ItemStat.Acceleration) / maxStatValue));
		StartCoroutine(animateProgress(handlingProgressbar, kartConfig.GetStat(KartSystem.ItemStat.Handling) / maxStatValue));
	}

	private IEnumerator animateProgress(UIProgressBar bar, float endVal)
	{
		float startVal = bar.value;
		float duration = 0.1f;
		Stopwatch sw = new Stopwatch(duration);
		while ((bool)sw)
		{
			yield return null;
			bar.value = Mathf.Lerp(startVal, endVal, sw.Progress);
		}
		bar.value = Mathf.Lerp(startVal, endVal, sw.Progress);
	}
}

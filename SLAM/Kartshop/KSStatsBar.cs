using System.Collections;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSStatsBar : MonoBehaviour
{
	[SerializeField]
	private UIProgressBar progressBar;

	[SerializeField]
	private float maxValue;

	[SerializeField]
	private KartSystem.ItemStat stat;

	private void OnEnable()
	{
		GameEvents.Subscribe<KSShopController.KartChangedEvent>(onKartChanged);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KSShopController.KartChangedEvent>(onKartChanged);
	}

	private void onKartChanged(KSShopController.KartChangedEvent evt)
	{
		float num = evt.Configuration.GetStat(stat);
		StopAllCoroutines();
		StartCoroutine(animateTo(num / maxValue));
	}

	private IEnumerator animateTo(float endVal)
	{
		float startVal = progressBar.value;
		float duration = 0.1f;
		Stopwatch sw = new Stopwatch(duration);
		while ((bool)sw)
		{
			yield return null;
			progressBar.value = Mathf.Lerp(startVal, endVal, sw.Progress);
		}
		progressBar.value = Mathf.Lerp(startVal, endVal, sw.Progress);
	}
}

using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRcountdownView : View
{
	[SerializeField]
	private UITweener redLightTween;

	[SerializeField]
	private UITweener orangeLightTween;

	[SerializeField]
	private UITweener greenLightTween;

	public void BeginCountdown(float totalDuration)
	{
		StartCoroutine(doCountdownRoutine(totalDuration));
	}

	private IEnumerator doCountdownRoutine(float totalDuration)
	{
		yield return new WaitForSeconds(totalDuration / 3f);
		redLightTween.PlayForward();
		AudioController.Play("KR_321_countdown");
		yield return new WaitForSeconds(totalDuration / 3f);
		orangeLightTween.PlayForward();
		yield return new WaitForSeconds(totalDuration / 3f);
		greenLightTween.PlayForward();
	}
}

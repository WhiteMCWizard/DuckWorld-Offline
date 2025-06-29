using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZThudView : HUDView
{
	[SerializeField]
	private UILabel lblCountDown;

	private int totalTruckCount;

	private int currentTruckCount;

	public void DoCountdown()
	{
		StartCoroutine(doCountDown());
	}

	private IEnumerator doCountDown()
	{
		string[] strs = new string[4]
		{
			"3",
			"2",
			"1",
			Localization.Get("ZT_COUNTDOWN_GO")
		};
		UITweener[] tweeners = lblCountDown.GetComponentsInChildren<UITweener>(includeInactive: true);
		lblCountDown.gameObject.SetActive(value: true);
		for (int i = 0; i < strs.Length; i++)
		{
			lblCountDown.text = strs[i];
			for (int j = 0; j < tweeners.Length; j++)
			{
				tweeners[j].ResetToBeginning();
				tweeners[j].PlayForward();
			}
			AudioController.Play((i >= strs.Length - 1) ? "countdown_last" : "countdown_first");
			yield return new WaitForSeconds(0.9f);
		}
		lblCountDown.gameObject.SetActive(value: false);
		GameEvents.Invoke(new ZooTransportGame.ZTCountdownFinishedEvent());
	}
}

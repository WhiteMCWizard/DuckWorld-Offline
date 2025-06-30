using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRicetutorialView : TutorialView
{
	[SerializeField]
	private ToolTip iceToolTip;

	[SerializeField]
	private Collider iceCollider;

	private KR_HumanKart kart;

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void onRaceStarted(KR_StartRaceEvent evt)
	{
		kart = Object.FindObjectOfType<KR_HumanKart>();
		StartCoroutine(explainIce());
	}

	private IEnumerator explainIce()
	{
		iceCollider.enabled = true;
		while (!iceCollider.bounds.Contains(kart.transform.position))
		{
			yield return null;
		}
		iceToolTip.Show();
		while (iceCollider.bounds.Contains(kart.transform.position))
		{
			yield return null;
		}
		iceToolTip.Hide();
	}
}

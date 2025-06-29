using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRtutorialView : TutorialView
{
	[SerializeField]
	private ToolTip controlsToolTip;

	[SerializeField]
	private ToolTip controlsPointerToolTip;

	[SerializeField]
	private ToolTip speedBoostToolTip;

	[SerializeField]
	private ToolTip heartToolTip;

	[SerializeField]
	private ToolTip cornerToolTip;

	[SerializeField]
	private Collider cornerCollider;

	private KR_HumanKart kart;

	private List<KRpickupBoost> speedBoosts;

	private KRpickupBoost speedBoost;

	private int boostsCollected;

	private List<KRpickupHealth> hearts;

	private KRpickupHealth heart;

	private int heartsCollected;

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_PickupBoostEvent>(onBoostCollected);
		GameEvents.Subscribe<KR_PickupEvent>(onHeartCollected);
		GameEvents.Subscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_PickupBoostEvent>(onBoostCollected);
		GameEvents.Unsubscribe<KR_PickupEvent>(onHeartCollected);
		GameEvents.Unsubscribe<KR_StartRaceEvent>(onRaceStarted);
	}

	private void onRaceStarted(KR_StartRaceEvent evt)
	{
		kart = Object.FindObjectOfType<KR_HumanKart>();
		Enumerable.IOrderedEnumerable<KRpickupBoost> collection = from c in Object.FindObjectsOfType<KRpickupBoost>()
			orderby Vector3.Distance(c.transform.position, kart.transform.position)
			select c;
		speedBoosts = collection.ToList();
		speedBoost = speedBoosts.First();
		Enumerable.IOrderedEnumerable<KRpickupHealth> collection2 = from h in Object.FindObjectsOfType<KRpickupHealth>()
			orderby Vector3.Distance(h.transform.position, kart.transform.position)
			select h;
		hearts = collection2.ToList();
		heart = hearts.First();
		StartCoroutine(step1());
	}

	private IEnumerator step1()
	{
		controlsPointerToolTip.Show(kart.transform);
		controlsToolTip.Show(controlsPointerToolTip.GO.transform);
		while (!SLAMInput.Provider.GetButton(SLAMInput.Button.UpOrAction) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Down) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Left) && !SLAMInput.Provider.GetButton(SLAMInput.Button.Right))
		{
			yield return null;
		}
		yield return new WaitForSeconds(2f);
		controlsToolTip.Hide();
		controlsPointerToolTip.Hide();
		StartCoroutine(step2());
		StartCoroutine(step3());
		StartCoroutine(step4());
	}

	private IEnumerator step2()
	{
		while (speedBoost == null)
		{
			yield return null;
		}
		while (Vector3.Distance(kart.transform.position, speedBoost.transform.position) > 25f)
		{
			yield return null;
		}
		speedBoostToolTip.Show(speedBoost.transform);
		while (!kartPassedObject(speedBoost.transform))
		{
			yield return null;
		}
		speedBoostToolTip.Hide();
		if (speedBoosts.Count > 1 && boostsCollected == 0)
		{
			speedBoosts.RemoveAt(0);
			speedBoost = speedBoosts.First();
			StartCoroutine(step2());
		}
	}

	private IEnumerator step3()
	{
		while (heart == null || Vector3.Distance(kart.transform.position, heart.transform.position) > 25f)
		{
			yield return null;
		}
		if (kart.HeartsLeft < 3)
		{
			heartToolTip.Show(heart.transform);
		}
		while (heart != null && !kartPassedObject(heart.transform))
		{
			yield return null;
		}
		heartToolTip.Hide();
		if (hearts.Count > 1 && heartsCollected == 0)
		{
			hearts.RemoveAt(0);
			heart = hearts.First();
			StartCoroutine(step3());
		}
	}

	private IEnumerator step4()
	{
		cornerCollider.enabled = true;
		while (!cornerCollider.bounds.Contains(kart.transform.position))
		{
			yield return null;
		}
		controlsPointerToolTip.Show(kart.transform);
		cornerToolTip.Show(controlsPointerToolTip.GO.transform);
		while (cornerCollider.bounds.Contains(kart.transform.position))
		{
			yield return null;
		}
		cornerToolTip.Hide();
		controlsPointerToolTip.Hide();
	}

	private void onBoostCollected(KR_PickupBoostEvent evt)
	{
		if (evt.Kart == kart)
		{
			boostsCollected++;
		}
	}

	private void onHeartCollected(KR_PickupEvent evt)
	{
		if (evt.Kart == kart)
		{
			heartsCollected++;
		}
	}

	private bool kartPassedObject(Transform obj)
	{
		return Vector3.Dot(kart.transform.forward.normalized, (obj.transform.position - kart.transform.position).normalized) < 0f;
	}
}

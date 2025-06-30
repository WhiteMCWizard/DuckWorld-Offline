using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTHudView : HUDView
{
	[SerializeField]
	private GameObject lblFeatherBonusPrefab;

	[SerializeField]
	private UILabel lblFeatherCount;

	[SerializeField]
	private float featherCountSpeed = 10f;

	private float targetFeatherCount;

	private float currentFeatherCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<FTTPickupCollectedEvent>(onPickupCollected);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FTTPickupCollectedEvent>(onPickupCollected);
	}

	private void onPickupCollected(FTTPickupCollectedEvent evt)
	{
		if (evt.Pickup.Type == FTTPickup.PickupType.Feather)
		{
			StartCoroutine(doAnimateTowardsUI(Controller<FTTGameController>().GetFeatherBonusPoints(), evt.Pickup.transform.position));
		}
	}

	private IEnumerator doAnimateTowardsUI(int bonusPoints, Vector3 worldStartPos)
	{
		GameObject gui = NGUITools.AddChild(base.gameObject, lblFeatherBonusPrefab);
		gui.GetComponent<UILabel>().text = "+" + bonusPoints;
		Vector3 startPos = convertWorldPosToScreen(worldStartPos);
		Vector3 endPos = lblFeatherCount.transform.position;
		gui.transform.position = startPos;
		Stopwatch sw = new Stopwatch(0.5f);
		while ((bool)sw)
		{
			yield return null;
			gui.transform.position = Vector3.Lerp(startPos, endPos, sw.Progress);
		}
		Object.Destroy(gui);
		targetFeatherCount += bonusPoints;
	}

	private Vector2 convertWorldPosToScreen(Vector3 worldpos)
	{
		return GetComponentInParent<Camera>().ScreenToWorldPoint(Camera.main.WorldToScreenPoint(worldpos));
	}

	protected override void Update()
	{
		if (currentFeatherCount < targetFeatherCount)
		{
			currentFeatherCount = Mathf.MoveTowards(currentFeatherCount, targetFeatherCount, featherCountSpeed * Time.deltaTime);
			lblFeatherCount.text = currentFeatherCount.ToString("0");
		}
	}
}

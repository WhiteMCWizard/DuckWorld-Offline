using System;
using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYSpotView : View
{
	[Serializable]
	public struct FYSpotViewIcon
	{
		public GameObject Prefab;

		public FruityardGame.FYTreeAction ActionType;
	}

	[SerializeField]
	private FYSpotViewIcon[] icons;

	[SerializeField]
	private Vector3[] iconOffset;

	[SerializeField]
	private GameObject timerPrefab;

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelCompletedEvent>(levelCompleted);
		FYSpot[] array = UnityEngine.Object.FindObjectsOfType<FYSpot>();
		foreach (FYSpot fYSpot in array)
		{
			updateSpotUI(fYSpot);
		}
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelCompletedEvent>(levelCompleted);
	}

	private void levelCompleted(FruityardGame.LevelCompletedEvent obj)
	{
		FYSpot[] array = UnityEngine.Object.FindObjectsOfType<FYSpot>();
		foreach (FYSpot fYSpot in array)
		{
			updateSpotUI(fYSpot);
		}
	}

	private void updateSpotUI(FYSpot fYSpot)
	{
		if (fYSpot.gameObject.activeInHierarchy)
		{
			StartCoroutine(doSpotUI(fYSpot));
		}
	}

	private IEnumerator doSpotUI(FYSpot spot)
	{
		yield return null;
		if (spot.gameObject.activeInHierarchy)
		{
			Vector3 newPos = UICamera.currentCamera.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(spot.transform.position + iconOffset[spot.GetGrowPhase()]));
			newPos.z = 0f;
			int action = spot.RequiredActionIndex;
			FYSpot spot2 = default(FYSpot);
			GameObject obj = UnityEngine.Object.Instantiate(icons.First((FYSpotViewIcon i) => i.ActionType == spot2.CurrentRequiredAction).Prefab);
			obj.transform.parent = base.transform;
			obj.transform.position = newPos;
			obj.transform.localScale = Vector3.one;
			obj.GetComponentInChildren<FYIconListener>().spot = spot;
			UISprite timerWidget = UnityEngine.Object.Instantiate(timerPrefab).GetComponent<UISprite>();
			timerWidget.transform.parent = obj.transform;
			timerWidget.transform.localPosition = Vector3.down * 90f;
			timerWidget.transform.localScale = Vector3.one;
			while (spot.RequiredActionIndex == action)
			{
				timerWidget.fillAmount = 1f - spot.CurrentActionTimer.Progress;
				yield return null;
			}
			UnityEngine.Object.Destroy(obj);
			if (spot.RequiredActionIndex < spot.RequiredActions.Length)
			{
				float waitingTime = ((spot.RequiredActionIndex <= action) ? 0.3f : 3f);
				yield return new WaitForSeconds(waitingTime);
				StartCoroutine(doSpotUI(spot));
			}
		}
	}
}

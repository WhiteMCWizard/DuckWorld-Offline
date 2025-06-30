using System;
using System.Collections;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Fruityard;

public class FYHudView : HUDView
{
	[Serializable]
	public struct FYHudTreeIcon
	{
		public GameObject Prefab;

		public FruityardGame.FYTreeType TreeType;
	}

	[SerializeField]
	private FYHudTreeIcon[] icons;

	[SerializeField]
	private UIGrid gridParent;

	[SerializeField]
	private float fruitShowDelay = 0.5f;

	private void OnEnable()
	{
		GameEvents.Subscribe<FruityardGame.LevelInitEvent>(onLevelInit);
		GameEvents.Subscribe<FruityardGame.TreeCompletedEvent>(onTreeCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FruityardGame.LevelInitEvent>(onLevelInit);
		GameEvents.Unsubscribe<FruityardGame.TreeCompletedEvent>(onTreeCompleted);
	}

	public IEnumerator ShowPickupList()
	{
		yield return null;
	}

	private void onLevelInit(FruityardGame.LevelInitEvent evt)
	{
		for (int i = 0; i < gridParent.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(gridParent.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < evt.PickupList.Count; j++)
		{
			FruityardGame.FYTreeType treeType = evt.PickupList[j];
			GameObject prefab = icons.First((FYHudTreeIcon ic) => ic.TreeType == treeType).Prefab;
			Transform transform = UnityEngine.Object.Instantiate(prefab).transform;
			transform.GetComponent<UITweener>().delay = (float)j * fruitShowDelay;
			transform.GetComponent<TweenAlpha>().delay = (float)j * fruitShowDelay;
			if (j + 1 >= evt.PickupList.Count)
			{
				transform.GetComponent<UITweener>().onFinished.Add(new EventDelegate(doneTweening));
			}
			gridParent.AddChild(transform);
			transform.transform.localScale = Vector3.one;
			transform.name = treeType.ToString();
			gridParent.Reposition();
			gridParent.repositionNow = true;
		}
	}

	private void doneTweening()
	{
		GameEvents.Invoke(new FruityardGame.LevelStartedEvent());
	}

	private void onTreeCompleted(FruityardGame.TreeCompletedEvent evt)
	{
		foreach (Transform item in gridParent.transform)
		{
			if (item.name.Equals(evt.Tree.Type.ToString()) && !item.GetChild(0).gameObject.activeSelf)
			{
				item.GetChild(0).gameObject.SetActive(value: true);
				break;
			}
		}
	}
}

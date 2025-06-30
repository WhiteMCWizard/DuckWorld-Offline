using System;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.CraneOperator;

public class CraneOperatorCrateView : View
{
	[Serializable]
	public class CrateHudIcon
	{
		public Crate.CrateType Type = Crate.CrateType.Elephant;

		public string SpriteName;
	}

	private class CrateToggle
	{
		public Crate.CrateType CrateType;

		public UIToggle Toggle;
	}

	[SerializeField]
	private GameObject animalTogglePrefab;

	[SerializeField]
	private UISprite sprtBackground;

	[SerializeField]
	private UIGrid grd;

	[SerializeField]
	private CrateHudIcon[] hudIcon;

	[SerializeField]
	private GameObject[] objectsToDisable;

	private List<CrateToggle> toggles = new List<CrateToggle>();

	public void InitialisePickupList(Crate[] pickupList)
	{
		foreach (CrateToggle toggle in toggles)
		{
			UnityEngine.Object.Destroy(toggle.Toggle.gameObject);
		}
		sprtBackground.width = 20 + pickupList.Length * 34;
		toggles.Clear();
		int num = 0;
		Crate crate;
		for (int i = 0; i < pickupList.Length; i++)
		{
			crate = pickupList[i];
			UIToggle uIToggle = InstantiateAnimalToggle(base.transform, hudIcon.FirstOrDefault((CrateHudIcon hi) => hi.Type == crate.Type).SpriteName, new Vector3(10 + num++ * 34, 0f));
			uIToggle.name = "Crate-" + crate.name;
			toggles.Add(new CrateToggle
			{
				CrateType = crate.Type,
				Toggle = uIToggle
			});
		}
		EnableHUD();
		Invoke("RepositionGrid", 0f);
	}

	private void RepositionGrid()
	{
		grd.Reposition();
	}

	private void EnableHUD()
	{
		Invoke("reallyEnableHud", 1f);
	}

	private void reallyEnableHud()
	{
		base.gameObject.SetActive(value: true);
		for (int i = 0; i < toggles.Count; i++)
		{
			toggles[i].Toggle.gameObject.SetActive(value: true);
		}
		for (int j = 0; j < objectsToDisable.Length; j++)
		{
			objectsToDisable[j].SetActive(value: true);
		}
		RepositionGrid();
	}

	private void DisableHUD()
	{
		base.gameObject.SetActive(value: false);
		for (int i = 0; i < toggles.Count; i++)
		{
			toggles[i].Toggle.gameObject.SetActive(value: false);
		}
		for (int j = 0; j < objectsToDisable.Length; j++)
		{
			objectsToDisable[j].SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		DisableHUD();
	}

	public void UpdateUI(KeyValuePair<Crate, bool>[] status)
	{
		List<CrateToggle> list = new List<CrateToggle>(toggles);
		for (int i = 0; i < status.Length; i++)
		{
			KeyValuePair<Crate, bool> keyValuePair = status[i];
			foreach (CrateToggle item in list)
			{
				if (item.CrateType == keyValuePair.Key.Type)
				{
					item.Toggle.value = keyValuePair.Value;
					list.Remove(item);
					break;
				}
			}
		}
	}

	private UIToggle InstantiateAnimalToggle(Transform parent, string spritename, Vector3 localPos)
	{
		GameObject gameObject = NGUITools.AddChild(parent.gameObject, animalTogglePrefab);
		gameObject.transform.localPosition = localPos;
		gameObject.GetComponent<UISprite>().spriteName = spritename;
		UIToggle component = gameObject.GetComponent<UIToggle>();
		component.value = false;
		return component;
	}
}

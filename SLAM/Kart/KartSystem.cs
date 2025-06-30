using System.Collections.Generic;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart;

public static class KartSystem
{
	public enum ItemCategory
	{
		Colors,
		Wheels,
		SteeringWheels,
		Bodies,
		Spoilers,
		Stickers
	}

	public enum ItemStat
	{
		TopSpeed,
		Handling,
		Acceleration
	}

	public static KartConfigurationData PlayerKartConfiguration { get; set; }

	public static GameObject SpawnKart(KartConfigurationData config)
	{
		GameObject gameObject = new GameObject("Kart");
		UpdateKart(gameObject, config);
		return gameObject;
	}

	public static void UpdateKart(GameObject parent, KartConfigurationData config)
	{
		parent.transform.DestroyChildren();
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		IEnumerable<KartItemLibrary.KartItem> enumerable = config.Items.Select((KartConfigurationData.KartConfigItem kartConfigItem) => itemLibrary.GetItemByGUID(kartConfigItem.GUID));
		KartItemLibrary.KartItem kartItem = enumerable.First((KartItemLibrary.KartItem i) => i.Category == ItemCategory.Bodies);
		GameObject gameObject = spawnItem(parent, kartItem, config.GetPrimaryColor(kartItem.GUID), config.GetSecondaryColor(kartItem.GUID));
		KartBodyAnchor componentInChildren = gameObject.GetComponentInChildren<KartBodyAnchor>();
		KartItemLibrary.KartItem kartItem2 = enumerable.First((KartItemLibrary.KartItem i) => i.Category == ItemCategory.Stickers);
		for (int num = 0; num < gameObject.transform.childCount; num++)
		{
			Transform child = gameObject.transform.GetChild(num);
			if (child.name.StartsWith("KS_Decal"))
			{
				if (child.name == kartItem2.Prefab.name)
				{
					child.gameObject.SetActive(value: true);
					Renderer component = child.GetComponent<Renderer>();
					component.material.SetColor("_RedColor", config.GetPrimaryColor(kartItem2.GUID));
					component.material.SetColor("_GreenColor", config.GetSecondaryColor(kartItem2.GUID));
				}
				else
				{
					child.gameObject.SetActive(value: false);
				}
			}
		}
		foreach (KartItemLibrary.KartItem item in enumerable)
		{
			foreach (Transform anchor in componentInChildren.GetAnchors(item.Category))
			{
				spawnItem(anchor.gameObject, item, config.GetPrimaryColor(item.GUID), config.GetSecondaryColor(item.GUID));
			}
		}
	}

	private static GameObject spawnItem(GameObject parent, KartItemLibrary.KartItem item, Color primaryColor, Color secondaryColor)
	{
		GameObject gameObject = Object.Instantiate(item.Prefab, parent.transform.position, parent.transform.rotation) as GameObject;
		gameObject.transform.parent = parent.transform;
		gameObject.name = item.GUID;
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material.HasProperty("_RedColor") && material.HasProperty("_GreenColor"))
				{
					material.SetColor("_RedColor", primaryColor);
					material.SetColor("_GreenColor", secondaryColor);
				}
			}
			renderer.materials = materials;
		}
		return gameObject;
	}
}

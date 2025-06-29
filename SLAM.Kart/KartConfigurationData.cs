using System;
using System.Collections.Generic;
using LitJson;
using SLAM.KartRacing;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Kart;

[Serializable]
public class KartConfigurationData : ICloneable
{
	[Serializable]
	public struct KartConfigItem
	{
		[KartItemLibrary.KartItemGUID]
		public string GUID;

		public int PrimaryColorPaletteIndex;

		public int SecondaryColorPaletteIndex;
	}

	[JsonName("id")]
	[HideInInspector]
	public int id;

	[JsonName("active")]
	public bool active;

	[JsonName("config")]
	public KartConfigItem[] Items;

	public Color GetPrimaryColor(string guid)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		return itemLibrary.PrimaryColorPalette[Items.First((KartConfigItem kc) => kc.GUID == guid).PrimaryColorPaletteIndex];
	}

	public Color GetSecondaryColor(string guid)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		return itemLibrary.SecondaryColorPalette[Items.First((KartConfigItem kc) => kc.GUID == guid).SecondaryColorPaletteIndex];
	}

	public void SetPrimaryColor(string guid, Color color)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].GUID == guid)
			{
				Items[i].PrimaryColorPaletteIndex = Array.IndexOf(itemLibrary.PrimaryColorPalette, color);
			}
		}
	}

	public void SetSecondaryColor(string guid, Color color)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].GUID == guid)
			{
				Items[i].SecondaryColorPaletteIndex = Array.IndexOf(itemLibrary.SecondaryColorPalette, color);
			}
		}
	}

	public float GetStat(KartSystem.ItemStat stat, KRPhysicsMaterialType materialType = KRPhysicsMaterialType.Dirt)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		return Items.Select((KartConfigItem kartItem) => itemLibrary.GetItemByGUID(kartItem.GUID).GetStat(stat, materialType)).Sum();
	}

	public bool HasSnowItem()
	{
		KartItemLibrary lib = KartItemLibrary.GetItemLibrary();
		return Items.Any((KartConfigItem it) => lib.GetItemByGUID(it.GUID).Snow);
	}

	public bool HasOilItem()
	{
		KartItemLibrary lib = KartItemLibrary.GetItemLibrary();
		return Items.Any((KartConfigItem it) => lib.GetItemByGUID(it.GUID).Oil);
	}

	public bool ReplaceItem(KartItemLibrary.KartItem newItem)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		for (int i = 0; i < Items.Length; i++)
		{
			KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(Items[i].GUID);
			if (itemByGUID != null && itemByGUID.Category == newItem.Category)
			{
				bool result = Items[i].GUID != newItem.GUID;
				Items[i].GUID = newItem.GUID;
				return result;
			}
		}
		List<KartConfigItem> list = new List<KartConfigItem>(Items);
		list.Add(new KartConfigItem
		{
			GUID = newItem.GUID,
			PrimaryColorPaletteIndex = 0,
			SecondaryColorPaletteIndex = 0
		});
		Items = list.ToArray();
		return true;
	}

	public bool HasItem(string guid)
	{
		for (int i = 0; i < Items.Length; i++)
		{
			if (Items[i].GUID == guid)
			{
				return true;
			}
		}
		return false;
	}

	public KartItemLibrary.KartItem GetItem(KartSystem.ItemCategory category)
	{
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		KartConfigItem[] items = Items;
		for (int i = 0; i < items.Length; i++)
		{
			KartConfigItem kartConfigItem = items[i];
			KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(kartConfigItem.GUID);
			if (itemByGUID.Category == category)
			{
				return itemByGUID;
			}
		}
		return null;
	}

	public object Clone()
	{
		KartConfigurationData kartConfigurationData = new KartConfigurationData();
		kartConfigurationData.id = id;
		kartConfigurationData.active = active;
		kartConfigurationData.Items = new KartConfigItem[Items.Length];
		Array.Copy(Items, kartConfigurationData.Items, Items.Length);
		return kartConfigurationData;
	}

	public override string ToString()
	{
		return string.Format("|{0}|{1}|{2}|", "id=" + id, "active?" + active, "itemcount=" + Items.Length);
	}
}

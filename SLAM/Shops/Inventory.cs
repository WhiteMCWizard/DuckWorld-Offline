using System;
using System.Collections.Generic;
using SLAM.Avatar;
using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops;

public class Inventory : MonoBehaviour
{
	[Serializable]
	public class Filter
	{
		[HideInInspector]
		public AvatarSystem.Gender Gender;

		[HideInInspector]
		public AvatarSystem.Race Race;

		[Tooltip("Will be ignored when \"OnlyShowBoughtItems\" is checked.")]
		public int ShopId = -1;

		public List<AvatarSystem.ItemCategory> Categories;

		[Tooltip("Show items that are hidden; this used to show Santa Claus suits in your wardrobe after the Xmax period.")]
		public bool ShowHiddenItems;

		public bool OnlyShowBoughtItems = true;
	}

	protected ShopCategoryDefinition[] categoryDefinitions;

	public ShopCategoryDefinition[] Items => categoryDefinitions;

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
	}

	public virtual void RetrieveInventory(Filter filter, Action ready)
	{
		if (filter.OnlyShowBoughtItems)
		{
			processShopItems(filter, ShopItems.All, ready);
		}
		else
		{
			ApiClient.GetShopItems(filter.ShopId, delegate(ShopData shopData)
			{
				processShopItems(filter, shopData.Items, ready);
			});
		}
	}

	protected void processShopItems(Filter filter, ShopItemData[] items, Action callback)
	{
		// Replace ApiClient.GetPlayerPurchasedShopItems with local implementation
		if (SaveManager.Instance.IsLoaded)
		{
			// Get purchased items from local save data
			PurchasedShopItemData[] purchasedItems = SaveManager.Instance.GetSaveData().purchasedShopItems;

			AvatarItemLibrary itemLibrary = AvatarItemLibrary.GetItemLibrary(filter.Race, filter.Gender);
			Dictionary<AvatarSystem.ItemCategory, Dictionary<string, List<ShopLibraryItem>>> dictionary = new Dictionary<AvatarSystem.ItemCategory, Dictionary<string, List<ShopLibraryItem>>>();
			ShopItemData[] array = items;
			foreach (ShopItemData shopItemData in array)
			{
				if (shopItemData.VisibleInShop || filter.ShowHiddenItems)
				{
					AvatarItemLibrary.AvatarItem itemByGUID = itemLibrary.GetItemByGUID(shopItemData.GUID);
					if (itemByGUID != null && filter.Categories.Contains(itemByGUID.Category))
					{
						if (!dictionary.ContainsKey(itemByGUID.Category))
						{
							dictionary.Add(itemByGUID.Category, new Dictionary<string, List<ShopLibraryItem>>());
						}
						if (!dictionary[itemByGUID.Category].ContainsKey(itemByGUID.MeshName))
						{
							dictionary[itemByGUID.Category].Add(itemByGUID.MeshName, new List<ShopLibraryItem>());
						}
						dictionary[itemByGUID.Category][itemByGUID.MeshName].Add(new ShopLibraryItem
						{
							ShopItem = shopItemData,
							LibraryItem = itemByGUID
						});
					}
				}
			}
			List<ShopCategoryDefinition> list = new List<ShopCategoryDefinition>();
			foreach (AvatarSystem.ItemCategory category in filter.Categories)
			{
				if (dictionary.ContainsKey(category))
				{
					string empty = string.Empty;
					empty = category switch
					{
						AvatarSystem.ItemCategory.Legs => "Shop_FS_icon_bottom", 
						AvatarSystem.ItemCategory.Feet => "Shop_FS_icon_shoe", 
						_ => "Shop_FS_icon_top", 
					};
					ShopCategoryDefinition shopCategoryDefinition = new ShopCategoryDefinition
					{
						SpriteName = empty
					};
					List<ShopVariationDefinition> list2 = new List<ShopVariationDefinition>();
					foreach (KeyValuePair<string, List<ShopLibraryItem>> item in dictionary[category])
					{
						foreach (ShopLibraryItem vari in item.Value)
						{
							bool flag = purchasedItems.FirstOrDefault((PurchasedShopItemData a) => a.ShopItemId == vari.ShopItem.Id) != null;
							if (filter.OnlyShowBoughtItems == flag)
							{
								list2.Add(new ShopVariationDefinition
								{
									Item = vari,
									HasBeenBoughtByPlayer = flag
								});
							}
						}
					}
					shopCategoryDefinition.Items = list2.ToArray();
					if (list2.Count > 0)
					{
						list.Add(shopCategoryDefinition);
					}
				}
			}
			categoryDefinitions = list.ToArray();
			callback();
		}
		else
		{
			Debug.LogError("SaveManager is not loaded. Cannot retrieve purchased shop items.");
			callback();
		}
	}
}

using System;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.Kart;
using SLAM.SaveSystem;
using SLAM.Shops;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSShop : MonoBehaviour
{
	[Serializable]
	public class KSFilter
	{
		[Tooltip("Will be ignored when \"OnlyShowBoughtItems\" is checked.")]
		public int ShopId = -1;

		public List<KartSystem.ItemCategory> Categories;

		[Tooltip("Show items that are hidden; this used to show Santa Claus suits in your wardrobe after the Xmax period.")]
		public bool ShowHiddenItems;

		public bool OnlyShowBoughtItems = true;
	}

	[SerializeField]
	protected KSFilter filter;

	protected KSShopCategoryDefinition[] categoryDefinitions;

	protected List<KSShopItemDefinition> shoppingCart;

	private ShopData rawShopData;

	public KSShopCategoryDefinition[] Items => categoryDefinitions;

	public int ShoppingCartValue
	{
		get
		{
			int num = 0;
			for (int i = 0; i < shoppingCart.Count; i++)
			{
				num += shoppingCart[i].Item.ShopItem.Price;
			}
			return num;
		}
	}

	public KSShopItemDefinition[] ShoppingCart => shoppingCart.ToArray();

	public ShopData RawShopData => rawShopData;

	protected virtual void Start()
	{
		shoppingCart = new List<KSShopItemDefinition>();
		if (!SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			return;
		}
		if (AudioController.GetCategory("Music") != null && AudioController.GetCategory("Music").AudioItems.Length > 0)
		{
			AudioItem[] audioItems = AudioController.GetCategory("Music").AudioItems;
			foreach (AudioItem audioItem in audioItems)
			{
				AudioController.Play(audioItem.Name);
			}
		}
		else
		{
			Debug.LogWarning("Hey buddy, this game doesn't have music? Make sure there is an AudioController with a category 'Music'!");
		}
		if (AudioController.GetCategory("Ambience") != null && AudioController.GetCategory("Ambience").AudioItems.Length > 0)
		{
			AudioItem[] audioItems2 = AudioController.GetCategory("Ambience").AudioItems;
			foreach (AudioItem audioItem2 in audioItems2)
			{
				AudioController.Play(audioItem2.Name);
			}
		}
		else
		{
			Debug.LogWarning("Hey buddy, this game doesn't have ambience sounds? Make sure there is an AudioController with a category 'Ambience'!");
		}
	}

	protected virtual void Update()
	{
	}

	public virtual void RetrieveInventory(Action ready)
	{
		rawShopData = AllShops.Kartshop;
		processShopItems(filter, rawShopData.Items, ready);
	}

	public void AddToCart(KSShopItemDefinition item)
	{
		for (int num = shoppingCart.Count - 1; num >= 0; num--)
		{
			if (shoppingCart[num].Item.LibraryItem.Category == item.Item.LibraryItem.Category)
			{
				shoppingCart.RemoveAt(num);
			}
		}
		if (!item.Item.HasBeenBought && !shoppingCart.Contains(item))
		{
			shoppingCart.Add(item);
		}
	}

	public void RemoveFromCart(KSShopItemDefinition item)
	{
		if (shoppingCart.Contains(item))
		{
			shoppingCart.Remove(item);
		}
	}

	public void ClearCart()
	{
		shoppingCart.Clear();
	}

	public void PurchaseShoppingCartContents(Action<Shop.Feedback> callback)
	{
		AudioController.Play("Avatar_clothes_buyItems");
		int[] array = new int[shoppingCart.Count];
		for (int i = 0; i < shoppingCart.Count; i++)
		{
			if (!shoppingCart[i].Item.HasBeenBought)
			{
				array[i] = shoppingCart[i].Item.ShopItem.Id;
				continue;
			}
			callback(new Shop.Feedback(succes: false, Localization.Get("WR_ERROR_ITEM_ALREADY_BOUGHT") + " " + shoppingCart[i].Item.ShopItem.Title));
			return;
		}
		ApiClient.PurchaseItems(array, filter.ShopId, delegate(bool succes)
		{
			if (succes)
			{
				foreach (KSShopItemDefinition item in shoppingCart)
				{
					GameEvents.Invoke(new TrackingEvent
					{
						Type = TrackingEvent.TrackingType.ItemBought,
						Arguments = new Dictionary<string, object>
						{
							{
								"ItemGUID",
								item.Item.LibraryItem.GUID
							},
							{
								"Price",
								item.Item.ShopItem.Price
							}
						}
					});
					item.Item.HasBeenBought = true;
				}
				shoppingCart.Clear();
				callback(new Shop.Feedback(succes: true, string.Empty));
			}
			else
			{
				callback(new Shop.Feedback(succes: false, StringFormatter.GetLocalizationFormatted("WR_ERROR_PURCHASE_FAILED", shoppingCart.Count)));
			}
		});
	}

	public static void GetUserKart(Action<KartConfigurationData> callback)
	{
		ApiClient.GetKartConfigurations(delegate(KartConfigurationData[] karts)
		{
			if (karts.Length == 0)
			{
				KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
				KartConfigurationData kartConfigurationData = (KartConfigurationData)itemLibrary.DefaultConfigurations.Last().Clone();
				kartConfigurationData.id = -1;
				kartConfigurationData.active = true;
				BuyKart(kartConfigurationData, 0, delegate(KartConfigurationData newKart)
				{
					callback(newKart);
				});
			}
			else
			{
				KartConfigurationData obj = null;
				for (int num = 0; num < karts.Length; num++)
				{
					if (karts[num].active)
					{
						obj = karts[num];
					}
				}
				callback(obj);
			}
		});
	}

	public static void BuyKart(KartConfigurationData kart, int price, Action<KartConfigurationData> callback)
	{
		ApiClient.GetWalletTotal(delegate(int total)
		{
			if (total >= price)
			{
				ApiClient.AddToWallet(-price, delegate
				{
					List<int> list = new List<int>();
					list.AddRange(from si in AllShops.AllItems
						where kart.HasItem(si.GUID)
						select si.Id);
					
					// Replace ApiClient.AddItemsToInventory with local implementation
					if (SaveManager.Instance.IsLoaded)
					{
						var saveData = SaveManager.Instance.GetSaveData();
						bool success = true;
						
						try
						{
							// Add items to local inventory
							var itemList = saveData.purchasedShopItems.ToList();
							foreach (int itemId in list)
							{
								// Create new purchased item data
								var purchasedItem = new PurchasedShopItemData
								{
									ShopItemId = itemId,
								};
								
								// Add to purchased items list
								itemList.Add(purchasedItem);
							}
							saveData.purchasedShopItems = itemList.ToArray();
							
							// Save changes
							SaveManager.Instance.MarkDirty();
						}
						catch (System.Exception ex)
						{
							Debug.LogError($"Failed to add items to inventory: {ex.Message}");
							success = false;
						}
						
						// Continue with success callback
						if (success)
						{
							KartConfigurationData kartConfigurationData = (KartConfigurationData)kart.Clone();
							kartConfigurationData.id = -1;
							kartConfigurationData.active = true;
							ApiClient.SaveKartConfiguration(kartConfigurationData, new Texture2D(4, 4).EncodeToPNG(), delegate(KartConfigurationData config)
							{
								callback(config);
							});
							AudioController.Play("Avatar_clothes_buyItems");
						}
						else
						{
							Debug.Log("Failed adding items to inventory :(");
							callback(null);
						}
					}
					else
					{
						Debug.LogError("SaveManager is not loaded. Cannot add items to inventory.");
						callback(null);
					}
				});
			}
			else
			{
				Debug.LogError("No money: Want to spend " + price + " but have " + total);
				callback(null);
			}
		});
	}

	protected void processShopItems(KSFilter filter, ShopItemData[] webserviceItems, Action callback)
	{
		// Replace ApiClient.GetPlayerPurchasedShopItems with local implementation
		if (SaveManager.Instance.IsLoaded)
		{
			// Get purchased items from local save data
			PurchasedShopItemData[] purchasedItems = SaveManager.Instance.GetSaveData().purchasedShopItems;
			
			KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
			Dictionary<KartSystem.ItemCategory, List<KSShopLibraryItem>> dictionary = new Dictionary<KartSystem.ItemCategory, List<KSShopLibraryItem>>();
			List<KSShopCategoryDefinition> list = new List<KSShopCategoryDefinition>();
			ShopItemData[] array = webserviceItems;
			foreach (ShopItemData shopItem in array)
			{
				if (shopItem.VisibleInShop || filter.ShowHiddenItems)
				{
					KartItemLibrary.KartItem itemByGUID = itemLibrary.GetItemByGUID(shopItem.GUID);
					if (itemByGUID == null)
					{
						Debug.Log("Filter out " + shopItem.GUID);
					}
					else if (filter.Categories.Contains(itemByGUID.Category))
					{
						if (!dictionary.ContainsKey(itemByGUID.Category))
						{
							dictionary.Add(itemByGUID.Category, new List<KSShopLibraryItem>());
						}
						PurchasedShopItemData purchasedShopItemData = purchasedItems.FirstOrDefault((PurchasedShopItemData purchasedShopItemData2) => purchasedShopItemData2.ShopItemId == shopItem.Id);
						dictionary[itemByGUID.Category].Add(new KSShopLibraryItem
						{
							LibraryItem = itemByGUID,
							ShopItem = shopItem,
							HasBeenBought = (purchasedShopItemData != null)
						});
					}
				}
			}
			foreach (KartSystem.ItemCategory category in filter.Categories)
			{
				if (dictionary.ContainsKey(category))
				{
					KSShopCategoryDefinition kSShopCategoryDefinition = new KSShopCategoryDefinition
					{
						Category = category,
						SpriteName = category.ToString(),
						Items = dictionary[category].Select((KSShopLibraryItem item) => new KSShopItemDefinition
						{
							Item = item
						}).ToArray()
					};
					if (kSShopCategoryDefinition.Items.Count() > 0)
					{
						list.Add(kSShopCategoryDefinition);
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

using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.SaveSystem;
using SLAM.Shops;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Kartshop;

[RequireComponent(typeof(KSShop))]
public class KSShopController : ViewController
{
	public class KartChangedEvent
	{
		public KartConfigurationData Configuration;
	}

	[SerializeField]
	private View[] views;

	[SerializeField]
	private Transform turnTable;

	protected KSShop inventory;

	private int cashInWallet;

	private KartConfigurationData selectedKart;

	private KartConfigurationData previewKart;

	private GameObject currentKartGO;

	private KartSystem.ItemCategory selectedCategory = KartSystem.ItemCategory.Bodies;

	private KSShop shop => inventory;

	protected override void Start()
	{
		base.Start();
		AddViews(views);
		inventory = GetComponent<KSShop>();
		OpenView<LoadingView>();
		KSShop.GetUserKart(delegate(KartConfigurationData kart)
		{
			selectedKart = kart;
			inventory.RetrieveInventory(onInventoryRetrieved);
		});
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Subscribe<ShoppingbasketItemRemoveEvent>(onShoppingCartItemRemoved);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KSShopCategoryClickedEvent>(onCategoryClicked);
		GameEvents.Unsubscribe<ShoppingbasketItemRemoveEvent>(onShoppingCartItemRemoved);
	}

	public void OnSelectPart(KSShopItemDefinition item)
	{
		if (item.Item.HasBeenBought)
		{
			selectedKart.ReplaceItem(item.Item.LibraryItem);
		}
		inventory.AddToCart(item);
		syncShoppingBasket();
		previewKart.ReplaceItem(item.Item.LibraryItem);
		KartSystem.UpdateKart(currentKartGO, previewKart);
		KartChangedEvent kartChangedEvent = new KartChangedEvent();
		kartChangedEvent.Configuration = previewKart;
		GameEvents.Invoke(kartChangedEvent);
	}

	public void GoToHub()
	{
		if (shop.ShoppingCart.Length > 0)
		{
			ShowQuitEditingPopup();
		}
		else
		{
			doCloseEdit(saveKartConf: true);
		}
	}

	public void ShowBuyPartsPopup()
	{
		int shoppingCartValue = inventory.ShoppingCartValue;
		if (shoppingCartValue > cashInWallet)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), Localization.Get("KS_POPUP_NOT_ENOUGH_COINS"), Localization.Get("UI_OK"), null, null, null));
			return;
		}
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), StringFormatter.GetLocalizationFormatted("KS_CONFIRM_PURCHASE_PART", shoppingCartValue), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
		{
			buyShoppingbasket();
		}, null));
	}

	public void ShowQuitEditingPopup()
	{
		GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("KS_POPUP_ITEMS_IN_BASKET"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
		{
			doCloseEdit(saveKartConf: false);
		}, null));
	}

	public void SetColor(bool isPrimary, Color color)
	{
		KartItemLibrary.KartItem item = previewKart.GetItem(selectedCategory);
		if (isPrimary)
		{
			previewKart.SetPrimaryColor(item.GUID, color);
		}
		else
		{
			previewKart.SetSecondaryColor(item.GUID, color);
		}
		KartSystem.UpdateKart(currentKartGO, previewKart);
	}

	private void doCloseEdit(bool saveKartConf)
	{
		if (saveKartConf)
		{
			saveKart(previewKart);
			selectedKart = previewKart.Clone() as KartConfigurationData;
		}
		SceneManager.Load("Hub");
	}

	public void SwitchToColorMode()
	{
		CloseView<KSEditKartView>();
		KartItemLibrary itemLibrary = KartItemLibrary.GetItemLibrary();
		OpenView<KSSelectColorsView>().SetInfo(itemLibrary.PrimaryColorPalette, itemLibrary.SecondaryColorPalette, inventory.Items, previewKart, selectedCategory);
	}

	public void SwitchToEditMode()
	{
		CloseView<KSSelectColorsView>();
		OpenView<KSEditKartView>().UpdateCategoryParts(inventory.Items.ToArray(), selectedCategory);
	}

	private void onInventoryRetrieved()
	{
		if (SaveManager.Instance.IsLoaded)
		{
			// Get wallet total from local save data
			cashInWallet = SaveManager.Instance.GetSaveData().walletTotal;
			
			CloseView<LoadingView>();
			previewKart = selectedKart.Clone() as KartConfigurationData;
			spawnConfiguration(previewKart);
			OpenView<KSShopView>();
			OpenView<KSEditKartView>().UpdateSelection(previewKart);
			OpenView<SLAM.Shops.HUDView>().SetInfo(cashInWallet);
			OpenView<KSStatsView>().SetInfo(selectedKart);
			inventory.ClearCart();
			syncShop();
			syncShoppingBasket();
		}
		else
		{
			Debug.LogError("SaveManager is not loaded. Cannot retrieve wallet total.");
			CloseView<LoadingView>();
		}
	}

	private void onCategoryClicked(KSShopCategoryClickedEvent evt)
	{
		selectedCategory = evt.Data.Category;
	}

	private void onShoppingCartItemRemoved(ShoppingbasketItemRemoveEvent item)
	{
		previewKart.ReplaceItem(selectedKart.GetItem(item.RemovedItem.Item.LibraryItem.Category));
		spawnConfiguration(previewKart);
		GetView<KSEditKartView>().UpdateSelection(previewKart);
		syncShop();
		inventory.RemoveFromCart(item.RemovedItem);
		syncShoppingBasket();
		KartChangedEvent kartChangedEvent = new KartChangedEvent();
		kartChangedEvent.Configuration = previewKart;
		GameEvents.Invoke(kartChangedEvent);
	}

	private void saveKart(KartConfigurationData kart)
	{
		kart.active = true;
		SaveManager.Instance.GetSaveData().SaveKartConfiguration(kart, new Texture2D(4, 4).EncodeToPNG(), delegate(KartConfigurationData result)
		{
			KartSystem.PlayerKartConfiguration = result;
		});
	}

	private void spawnConfiguration(KartConfigurationData config)
	{
		if (currentKartGO != null)
		{
			Object.Destroy(currentKartGO);
		}
		GameObject gameObject = KartSystem.SpawnKart(config);
		gameObject.transform.parent = turnTable;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		currentKartGO = gameObject.gameObject;
	}

	private void syncShop()
	{
		GetView<KSEditKartView>().UpdateCategoryParts(inventory.Items.ToArray(), selectedCategory);
	}

	private void syncShoppingBasket()
	{
		GetView<KSShopView>().UpdateShoppingbasket(inventory.ShoppingCart);
	}

	private void buyShoppingbasket()
	{
		OpenView<LoadingView>();
		int costs = inventory.ShoppingCartValue;
		inventory.PurchaseShoppingCartContents(delegate(Shop.Feedback feedback)
		{
			if (costs <= cashInWallet)
			{
				if (feedback.WasSuccesfull)
				{
					GameEvents.Invoke(new TrackingEvent
					{
						Type = TrackingEvent.TrackingType.KartCustomizedEvent
					});
					selectedKart = previewKart.Clone() as KartConfigurationData;
					cashInWallet -= costs;
					GetView<SLAM.Shops.HUDView>().SetInfo(cashInWallet);
					syncShop();
					syncShoppingBasket();
				}
				else
				{
					GameEvents.Invoke(new PopupEvent("Error", feedback.Message, Localization.Get("UI_OK"), null, null, null));
				}
			}
			else
			{
				GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), Localization.Get("KS_POPUP_NOT_ENOUGH_COINS"), Localization.Get("UI_OK"), null, null, null));
			}
			CloseView<LoadingView>();
		});
	}
}

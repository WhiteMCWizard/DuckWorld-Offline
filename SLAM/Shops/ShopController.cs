using SLAM.Avatar;
using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Shops;

[RequireComponent(typeof(Shop))]
public class ShopController : InventoryController
{
	private int cashInWallet;

	private AvatarConfigurationData originalAvatarConfig;

	private Shop shop => inventory as Shop;

	protected override void Start()
	{
		base.Start();
		originalAvatarConfig = (AvatarConfigurationData)base.AvatarConfig.Clone();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameEvents.Subscribe<ShoppingCartItemRemovedEvent>(OnItemRemovedFromCart);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameEvents.Unsubscribe<ShoppingCartItemRemovedEvent>(OnItemRemovedFromCart);
	}

	public void PurchaseShoppingCartContents()
	{
		int totalAmount = shop.ShoppingCartValue;
		if (cashInWallet >= totalAmount)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), StringFormatter.GetLocalizationFormatted("WR_POPUP_TOTAL_ITEM_COSTS", totalAmount), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
			{
				OpenView<LoadingView>();
				shop.PurchaseShoppingCartContents(delegate(Shop.Feedback result)
				{
					if (result.WasSuccesfull)
					{
						ShopView view = GetView<ShopView>();
						cashInWallet -= totalAmount;
						view.UpdateWallet(cashInWallet.ToString());
						view.UpdateShoppingCart(new ShopVariationDefinition[0], 0.ToString());
						view.Load(shop.Items);
						ShopVariationDefinition[] shoppingCart = shop.ShoppingCart;
						foreach (ShopVariationDefinition shopVariationDefinition in shoppingCart)
						{
							originalAvatarConfig.ReplaceItem(shopVariationDefinition.Item.LibraryItem, avatarLibrary);
						}
						avatarAnimator.SetTrigger("SelectAvatar");
						AudioController.Play("Avatar_clothes_buyItems");
					}
					else
					{
						Debug.LogError("Purchase failed: " + result.Message);
					}
					CloseView<LoadingView>();
				}, filter);
			}, null));
		}
		else
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("WR_POPUP_TITLE_NOT_ENOUGH_COINS"), StringFormatter.GetLocalizationFormatted("WR_POPUP_NOT_ENOUGH_COINS", totalAmount, cashInWallet), Localization.Get("UI_OK"), null));
		}
	}

	public override void GoToHub()
	{
		if (shop.ShoppingCart.Length > 0)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_ARE_YOU_SURE"), Localization.Get("FS_POPUP_ITEMS_IN_BASKET"), Localization.Get("UI_YES"), Localization.Get("UI_NO"), delegate
			{
				ShopCategoryDefinition[] items = shop.Items;
				foreach (ShopCategoryDefinition shopCategoryDefinition in items)
				{
					ShopVariationDefinition[] items2 = shopCategoryDefinition.Items;
					foreach (ShopVariationDefinition shopVariationDefinition in items2)
					{
						string[] items3 = base.AvatarConfig.Items;
						foreach (string text in items3)
						{
							if (shopVariationDefinition.Item.LibraryItem.GUID == text && shopVariationDefinition.HasBeenBoughtByPlayer)
							{
								originalAvatarConfig.ReplaceItem(shopVariationDefinition.Item.LibraryItem, avatarLibrary);
							}
						}
					}
				}
				base.AvatarConfig = (AvatarConfigurationData)originalAvatarConfig.Clone();
				base.GoToHub();
			}, null));
		}
		else
		{
			base.GoToHub();
		}
	}

	protected override void OnInventoryRetrieved()
	{
		ApiClient.GetWalletTotal(delegate(int total)
		{
			base.OnInventoryRetrieved();
			cashInWallet = total;
			ShopView view = GetView<ShopView>();
			view.UpdateShoppingCart(shop.ShoppingCart, 0.ToString());
			view.UpdateWallet(cashInWallet.ToString());
		});
	}

	private void OnItemRemovedFromCart(ShoppingCartItemRemovedEvent evt)
	{
		shop.RemoveFromCart(evt.Removeditem);
		AvatarItemLibrary.AvatarItem itemByCategory = originalAvatarConfig.GetItemByCategory(evt.Removeditem.Item.LibraryItem.Category, avatarLibrary);
		base.AvatarConfig.ReplaceItem(itemByCategory, avatarLibrary);
		RefreshAvatar();
		GetView<ShopView>().UpdateShoppingCart(shop.ShoppingCart, shop.ShoppingCartValue.ToString());
		AudioController.Play("Avatar_clothes_removeItem");
	}

	protected override void OnVariationClicked(ShopVariationClickedEvent evt)
	{
		base.OnVariationClicked(evt);
		shop.AddToCart(evt.Data);
		GetView<ShopView>().UpdateShoppingCart(shop.ShoppingCart, shop.ShoppingCartValue.ToString());
	}
}

using SLAM.Avatar;
using UnityEngine;

namespace SLAM.Shops;

public class ShopView : InventoryView
{
	[SerializeField]
	private UIButton buyButton;

	[SerializeField]
	private UILabel walletLabel;

	[SerializeField]
	private UILabel totalLabel;

	[SerializeField]
	private Transform tShirtSlot;

	[SerializeField]
	private Transform pantsSlot;

	[SerializeField]
	private Transform shoesSlot;

	[SerializeField]
	private GameObject shopItemPrefab;

	public void UpdateWallet(string duckcoinsLeft)
	{
		walletLabel.text = duckcoinsLeft;
	}

	public void UpdateShoppingCart(ShopVariationDefinition[] cartContents, string totalValue)
	{
		bool flag = cartContents.Length > 0;
		buyButton.isEnabled = flag;
		buyButton.GetComponentsInChildren<UILabel>(includeInactive: true)[0].color = ((!flag) ? Color.gray : Color.white);
		tShirtSlot.DestroyChildren();
		pantsSlot.DestroyChildren();
		shoesSlot.DestroyChildren();
		Transform transform = null;
		for (int i = 0; i < cartContents.Length; i++)
		{
			switch (cartContents[i].Item.LibraryItem.Category)
			{
			case AvatarSystem.ItemCategory.Torso:
				transform = tShirtSlot;
				break;
			case AvatarSystem.ItemCategory.Legs:
				transform = pantsSlot;
				break;
			case AvatarSystem.ItemCategory.Feet:
				transform = shoesSlot;
				break;
			default:
				Debug.Log($"Hey buddy, I found unsupported item {cartContents[i].Item.ShopItem.InternalName} with category {cartContents[i].Item.LibraryItem.Category} in the shoppingcart, ignoring it.");
				break;
			}
			if (transform != null)
			{
				GameObject gameObject = NGUITools.AddChild(transform.gameObject, shopItemPrefab);
				gameObject.GetComponent<ShoppingCartItem>().Init(cartContents[i]);
			}
		}
		totalLabel.text = totalValue;
		RefreshVariations();
	}

	public void OnBuyClicked()
	{
		Controller<ShopController>().PurchaseShoppingCartContents();
	}
}

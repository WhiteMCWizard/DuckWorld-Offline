using UnityEngine;

namespace SLAM.Kartshop;

public class KSShoppingbasketItem : MonoBehaviour
{
	[SerializeField]
	private UITexture partIcon;

	[SerializeField]
	private GameObject pricetag;

	[SerializeField]
	private UILabel priceLabel;

	[SerializeField]
	private UISprite oilIcon;

	[SerializeField]
	private UISprite snowIcon;

	private KSShopItemDefinition data;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetData(KSShopItemDefinition data)
	{
		this.data = data;
		partIcon.mainTexture = data.Item.LibraryItem.Icon;
		pricetag.SetActive(!data.Item.HasBeenBought);
		priceLabel.text = this.data.Item.ShopItem.Price.ToString();
		oilIcon.cachedGameObject.SetActive(data.Item.LibraryItem.Oil);
		snowIcon.cachedGameObject.SetActive(data.Item.LibraryItem.Snow);
	}

	public void OnRemoveClicked()
	{
		AudioController.Play("Avatar_clothes_removeItem");
		ShoppingbasketItemRemoveEvent shoppingbasketItemRemoveEvent = new ShoppingbasketItemRemoveEvent();
		shoppingbasketItemRemoveEvent.RemovedItem = data;
		GameEvents.Invoke(shoppingbasketItemRemoveEvent);
	}
}

using UnityEngine;

namespace SLAM.Kartshop;

public class KSShopItem : MonoBehaviour
{
	[SerializeField]
	private UIButton button;

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

	public KSShopItemDefinition Data { get; private set; }

	public void Initialize(KSShopItemDefinition data)
	{
		Data = data;
		partIcon.mainTexture = data.Item.LibraryItem.Icon;
		pricetag.SetActive(!data.Item.HasBeenBought);
		priceLabel.text = data.Item.ShopItem.Price.ToString();
		oilIcon.cachedGameObject.SetActive(data.Item.LibraryItem.Oil);
		snowIcon.cachedGameObject.SetActive(data.Item.LibraryItem.Snow);
		button.isEnabled = true;
	}

	public void OnSelect()
	{
		KSShopItemClickedEvent kSShopItemClickedEvent = new KSShopItemClickedEvent();
		kSShopItemClickedEvent.Sender = this;
		kSShopItemClickedEvent.Data = Data;
		GameEvents.Invoke(kSShopItemClickedEvent);
	}

	public void SetSelected(bool state)
	{
		button.isEnabled = !state;
	}
}

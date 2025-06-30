using SLAM.Engine;
using SLAM.Kart;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSShopView : View
{
	[SerializeField]
	private UIButton buyButton;

	[SerializeField]
	private UIButton paintButton;

	[SerializeField]
	private UIButton editButton;

	[SerializeField]
	private UILabel totalShoppingbasketAmountLabel;

	[SerializeField]
	private Transform bodyParent;

	[SerializeField]
	private Transform wheelParent;

	[SerializeField]
	private Transform steeringwheelParent;

	[SerializeField]
	private Transform spoilerParent;

	[SerializeField]
	private Transform stickerParent;

	[SerializeField]
	private GameObject shopitemPrefab;

	public override void Init(ViewController controller)
	{
		base.Init(controller);
		editButton.isEnabled = false;
	}

	public void UpdateShoppingbasket(KSShopItemDefinition[] items)
	{
		bool flag = items.Length > 0;
		buyButton.isEnabled = flag;
		buyButton.GetComponentInChildren<UILabel>().color = ((!flag) ? Color.grey : Color.white);
		bodyParent.DestroyChildren();
		wheelParent.DestroyChildren();
		steeringwheelParent.DestroyChildren();
		spoilerParent.DestroyChildren();
		stickerParent.DestroyChildren();
		int num = 0;
		for (int i = 0; i < items.Length; i++)
		{
			Transform transform = null;
			switch (items[i].Item.LibraryItem.Category)
			{
			case KartSystem.ItemCategory.Wheels:
				transform = wheelParent;
				break;
			case KartSystem.ItemCategory.SteeringWheels:
				transform = steeringwheelParent;
				break;
			case KartSystem.ItemCategory.Spoilers:
				transform = spoilerParent;
				break;
			case KartSystem.ItemCategory.Stickers:
				transform = stickerParent;
				break;
			case KartSystem.ItemCategory.Bodies:
				transform = bodyParent;
				break;
			}
			if (transform != null)
			{
				GameObject gameObject = NGUITools.AddChild(transform.gameObject, shopitemPrefab);
				gameObject.GetComponent<KSShoppingbasketItem>().SetData(items[i]);
			}
			num += items[i].Item.ShopItem.Price;
		}
		totalShoppingbasketAmountLabel.text = num.ToString();
	}

	public void OnBuyClicked()
	{
		Controller<KSShopController>().ShowBuyPartsPopup();
	}

	public void OnHomeClicked()
	{
		Controller<KSShopController>().GoToHub();
	}

	public void OnSwitchToEditClicked()
	{
		editButton.isEnabled = false;
		paintButton.isEnabled = true;
		Controller<KSShopController>().SwitchToEditMode();
	}

	public void OnSwitchToPaintClicked()
	{
		editButton.isEnabled = true;
		paintButton.isEnabled = false;
		Controller<KSShopController>().SwitchToColorMode();
	}
}

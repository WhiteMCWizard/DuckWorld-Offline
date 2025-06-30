using SLAM.Kart;
using UnityEngine;

namespace SLAM.Kartshop;

public class KSShopCategory : MonoBehaviour
{
	[SerializeField]
	private UIToggle stateToggle;

	[SerializeField]
	private UIButton button;

	[SerializeField]
	private UISprite iconSelected;

	[SerializeField]
	private UISprite iconDeselected;

	public KSShopCategoryDefinition Data { get; private set; }

	public void Initialize(KSShopCategoryDefinition data)
	{
		Data = data;
		string text = string.Empty;
		switch (data.Category)
		{
		case KartSystem.ItemCategory.Wheels:
			text = "Shop_KR_icon_wheel";
			break;
		case KartSystem.ItemCategory.SteeringWheels:
			text = "Shop_KR_icon_steering_wheel";
			break;
		case KartSystem.ItemCategory.Bodies:
			text = "Shop_KR_icon_body";
			break;
		case KartSystem.ItemCategory.Spoilers:
			text = "Shop_KR_icon_spoiler";
			break;
		case KartSystem.ItemCategory.Stickers:
			text = "Shop_KR_icon_flame";
			break;
		}
		iconSelected.spriteName = text + "_active";
		iconDeselected.spriteName = text;
		iconSelected.MakePixelPerfect();
		iconDeselected.MakePixelPerfect();
	}

	public void SetSelected(bool state)
	{
		stateToggle.value = state;
		button.isEnabled = !state;
	}

	public void OnSelect()
	{
		KSShopCategoryClickedEvent kSShopCategoryClickedEvent = new KSShopCategoryClickedEvent();
		kSShopCategoryClickedEvent.Sender = this;
		kSShopCategoryClickedEvent.Data = Data;
		GameEvents.Invoke(kSShopCategoryClickedEvent);
	}
}

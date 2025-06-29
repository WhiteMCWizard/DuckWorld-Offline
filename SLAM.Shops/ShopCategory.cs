using UnityEngine;

namespace SLAM.Shops;

public class ShopCategory : MonoBehaviour
{
	[SerializeField]
	private UISprite icon;

	[SerializeField]
	private UIButton button;

	public ShopCategoryDefinition Data { get; private set; }

	public void Initialize(ShopCategoryDefinition data)
	{
		Data = data;
		icon.spriteName = Data.SpriteName + "_active";
		icon.MakePixelPerfect();
	}

	public void SetSelected(bool state)
	{
		string spriteName = Data.SpriteName;
		if (state)
		{
			spriteName = Data.SpriteName + "_active";
		}
		icon.spriteName = spriteName;
		button.isEnabled = !state;
	}

	public void OnSelect()
	{
		ShopCategoryClickedEvent shopCategoryClickedEvent = new ShopCategoryClickedEvent();
		shopCategoryClickedEvent.Sender = this;
		shopCategoryClickedEvent.Data = Data;
		GameEvents.Invoke(shopCategoryClickedEvent);
	}
}

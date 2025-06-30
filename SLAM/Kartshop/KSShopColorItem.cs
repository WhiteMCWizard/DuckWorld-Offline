using UnityEngine;

namespace SLAM.Kartshop;

public class KSShopColorItem : MonoBehaviour
{
	[SerializeField]
	private UISprite paintIcon;

	[SerializeField]
	private UIButton button;

	public ColorData Data { get; private set; }

	public void Initialize(ColorData data)
	{
		Data = data;
		paintIcon.color = data.Color;
	}

	public void SetSelected(bool state)
	{
		button.isEnabled = !state;
	}

	public void OnSelect()
	{
		KSShopColorItemClickedEvent kSShopColorItemClickedEvent = new KSShopColorItemClickedEvent();
		kSShopColorItemClickedEvent.Item = this;
		GameEvents.Invoke(kSShopColorItemClickedEvent);
	}
}

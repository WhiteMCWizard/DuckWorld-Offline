using SLAM.Avatar;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_Hair : MonoBehaviour
{
	[SerializeField]
	private UISprite hairSprite;

	[SerializeField]
	private UIToggle toggle;

	private AvatarItemLibrary.AvatarItem item;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetInfo(AvatarItemLibrary.AvatarItem item, bool isToggled)
	{
		this.item = item;
		toggle.value = isToggled;
		hairSprite.color = item.Material.GetColor("_GreenColor");
	}

	public void OnClicked()
	{
		HairClickedEvent hairClickedEvent = new HairClickedEvent();
		hairClickedEvent.Item = item;
		GameEvents.Invoke(hairClickedEvent);
	}
}

using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_Skin : MonoBehaviour
{
	[SerializeField]
	private UISprite skinSprite;

	[SerializeField]
	private UIToggle toggle;

	private Color color;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetInfo(Color color, bool isToggled)
	{
		skinSprite.color = (this.color = color);
		toggle.value = isToggled;
	}

	public void OnClicked()
	{
		SkinClickedEvent skinClickedEvent = new SkinClickedEvent();
		skinClickedEvent.Color = color;
		GameEvents.Invoke(skinClickedEvent);
	}
}

using SLAM.Engine;
using UnityEngine;

namespace SLAM.Smartphone;

public class NotificationCenterView : View
{
	[SerializeField]
	private UILabel notificationsLabel;

	[SerializeField]
	private UISprite notificationIcon;

	[SerializeField]
	private UITweener notificationTween;

	public override void Init(ViewController controller)
	{
		base.Init(controller);
		SetData(0);
	}

	public void SetData(int totalNotificationCount)
	{
		if (totalNotificationCount > 0)
		{
			notificationsLabel.text = totalNotificationCount.ToString();
			notificationTween.ResetToBeginning();
			notificationTween.PlayForward();
		}
		notificationIcon.cachedGameObject.SetActive(totalNotificationCount > 0);
	}

	public void OnOpenClicked()
	{
		Controller<SmartphoneController>().Show();
	}
}

using UnityEngine;

namespace SLAM.Smartphone;

public class AppIcon : MonoBehaviour
{
	[SerializeField]
	private UISprite icon;

	[SerializeField]
	private UILabel notificationsCountLabel;

	[SerializeField]
	private UISprite notificationIcon;

	private AppController app;

	private void Awake()
	{
		GameEvents.Subscribe<AppChangedEvent>(onAppChanged);
	}

	private void OnDestroy()
	{
		GameEvents.Unsubscribe<AppChangedEvent>(onAppChanged);
	}

	public void SetData(AppController appController)
	{
		app = appController;
		app.CreateIcon(icon);
		base.name = $"{app.SortingOrder}. {app}";
		setNotification(app.NotificationCount);
	}

	public void OnClick()
	{
		OpenAppRequestEvent openAppRequestEvent = new OpenAppRequestEvent();
		openAppRequestEvent.App = app;
		GameEvents.Invoke(openAppRequestEvent);
	}

	private void onAppChanged(AppChangedEvent evt)
	{
		if (evt.App == app)
		{
			setNotification(evt.App.NotificationCount);
		}
	}

	private void setNotification(int count)
	{
		notificationsCountLabel.text = count.ToString();
		notificationIcon.cachedGameObject.SetActive(count > 0);
	}
}

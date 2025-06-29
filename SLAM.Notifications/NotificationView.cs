using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Notifications;

public class NotificationView : MonoBehaviour
{
	[SerializeField]
	private GameObject notificationPrefab;

	[SerializeField]
	private GameObject parent;

	private Queue<NotificationEvent> notificationQueue = new Queue<NotificationEvent>();

	private NotificationEvent activeNotification;

	private void Start()
	{
		Object.DontDestroyOnLoad(base.transform.root.gameObject);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<NotificationEvent>(onNotificationReceivedEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<NotificationEvent>(onNotificationReceivedEvent);
	}

	private void onNotificationReceivedEvent(NotificationEvent evt)
	{
		notificationQueue.Enqueue(evt);
		updateQueue();
	}

	private void updateQueue()
	{
		if (notificationQueue.Count > 0 && activeNotification == null)
		{
			activeNotification = notificationQueue.Dequeue();
			GameObject gameObject = NGUITools.AddChild(parent, notificationPrefab);
			gameObject.GetComponent<Notification>().Show(activeNotification);
			CoroutineUtils.WaitForObjectDestroyed(gameObject, delegate
			{
				activeNotification = null;
				updateQueue();
			});
		}
	}
}

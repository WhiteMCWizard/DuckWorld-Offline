using UnityEngine;

namespace SLAM.Analytics;

// Analytics tracking is disabled for offline play.
// Event subscriptions are kept to prevent errors from code that fires TrackingEvents.
public class TrackingListener : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<TrackingEvent>(onTrackingEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrackingEvent>(onTrackingEvent);
	}

	private void onTrackingEvent(TrackingEvent evt)
	{
	}
}

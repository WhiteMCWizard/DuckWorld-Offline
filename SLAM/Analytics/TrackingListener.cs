using System.Collections.Generic;
using SLAM.BuildSystem;
using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Analytics;

public class TrackingListener : MonoBehaviour
{
	private GoogleAnalyticsV3 ga;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<TrackingEvent>(onTrackingEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrackingEvent>(onTrackingEvent);
	}

	private void onLogMessagesReceived(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception || type == LogType.Warning)
		{
			Application.ExternalCall("TRACKING.GAME.TrackException", condition, stackTrace, type.ToString());
		}
	}

	private void onTrackingEvent(TrackingEvent evt)
	{
	}

	private void trackSat(TrackingEvent evt)
	{
	}

	private void DoTrackEvent(GoogleAnalyticsV3 ga, string category, string action, string label, long value = 0, Dictionary<string, object> arguments = null)
	{
		ga.LogEvent(buildHit<EventHitBuilder>(arguments).SetEventCategory(category).SetEventAction(action).SetEventLabel(label)
			.SetEventValue(value));
	}

	private void DoTrackPageview(GoogleAnalyticsV3 ga, string url, Dictionary<string, object> arguments = null)
	{
		ga.LogScreen(buildHit<AppViewHitBuilder>(arguments).SetScreenName(url));
	}

	private string GetPageViewUrl(Dictionary<string, object> arguments, string suffix = "")
	{
		return string.Format("/game/{0}/{1}/{2}", arguments["LocationName"], arguments["GameName"], suffix);
	}

	private T buildHit<T>(Dictionary<string, object> arguments) where T : HitBuilder<T>, new()
	{
		T result = new T();
		if (arguments != null)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add("GameId", 13);
			dictionary.Add("Difficulty", 16);
			Dictionary<string, int> dictionary2 = dictionary;
			dictionary = new Dictionary<string, int>();
			dictionary.Add("Score", 3);
			dictionary.Add("Coins", 6);
			dictionary.Add("LoadingTime", 1);
			dictionary.Add("UserLoggedIn", 9);
			Dictionary<string, int> dictionary3 = dictionary;
			foreach (KeyValuePair<string, object> argument in arguments)
			{
				if (dictionary2.ContainsKey(argument.Key))
				{
					if (!result.GetCustomDimensions().ContainsKey(dictionary2[argument.Key]))
					{
						int dimensionNumber2 = dictionary2[argument.Key];
						string value2 = argument.Value.ToString();
						result.SetCustomDimension(dimensionNumber2, value2);
					}
				}
				else if (dictionary3.ContainsKey(argument.Key))
				{
					int metricNumber = dictionary3[argument.Key];
					string value3 = argument.Value.ToString();
					result.SetCustomMetric(metricNumber, value3);
				}
			}
		}
		return result;
	}
}

using System.Collections.Generic;
using SLAM.BuildSystem;
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
		if (ga == null)
		{
			DataStorage.GetWebConfiguration(delegate(WebConfiguration config)
			{
				ga = base.gameObject.AddComponent<GoogleAnalyticsV3>();
				ga.InitializeWithTrackingCode(config.GoogleAnalyticsTrackingCode, "Duckworld", "com.sanoma.duckworld", SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString());
				onTrackingEvent(evt);
			});
			return;
		}
		trackSat(evt);
		switch (evt.Type)
		{
		case TrackingEvent.TrackingType.HubOpened:
			DoTrackPageview(ga, "/game/hub");
			break;
		case TrackingEvent.TrackingType.LocationOpened:
			DoTrackPageview(ga, "/game/" + evt.Arguments["LocationName"]);
			break;
		case TrackingEvent.TrackingType.StartViewOpened:
			DoTrackPageview(ga, GetPageViewUrl(evt.Arguments, string.Empty), evt.Arguments);
			break;
		case TrackingEvent.TrackingType.GameStart:
			DoTrackPageview(ga, GetPageViewUrl(evt.Arguments, "game"), evt.Arguments);
			DoTrackEvent(ga, evt.Arguments["LocationName"].ToString(), string.Concat(evt.Arguments["GameName"], " start"), string.Concat(evt.Arguments["GameName"], " ", evt.Arguments["Difficulty"]), 0L, evt.Arguments);
			break;
		case TrackingEvent.TrackingType.GameQuit:
			DoTrackPageview(ga, GetPageViewUrl(evt.Arguments, "game"), evt.Arguments);
			DoTrackEvent(ga, evt.Arguments["LocationName"].ToString(), string.Concat(evt.Arguments["GameName"], " quit"), string.Concat(evt.Arguments["GameName"], " ", evt.Arguments["Difficulty"]), 0L, evt.Arguments);
			break;
		case TrackingEvent.TrackingType.GameWon:
			DoTrackPageview(ga, GetPageViewUrl(evt.Arguments, "game"), evt.Arguments);
			DoTrackEvent(ga, evt.Arguments["LocationName"].ToString(), string.Concat(evt.Arguments["GameName"], " won"), string.Concat(evt.Arguments["GameName"], " ", evt.Arguments["Difficulty"]), 0L, evt.Arguments);
			break;
		case TrackingEvent.TrackingType.GameLost:
			DoTrackPageview(ga, GetPageViewUrl(evt.Arguments, "game"), evt.Arguments);
			DoTrackEvent(ga, evt.Arguments["LocationName"].ToString(), string.Concat(evt.Arguments["GameName"], " lost"), string.Concat(evt.Arguments["GameName"], " ", evt.Arguments["Difficulty"]), 0L, evt.Arguments);
			break;
		case TrackingEvent.TrackingType.ItemBought:
			DoTrackEvent(ga, "Game", "Store", "buy-" + evt.Arguments["ItemGUID"], (int)evt.Arguments["Price"], evt.Arguments);
			break;
		case TrackingEvent.TrackingType.FriendshipRequested:
			DoTrackEvent(ga, "game", "profile", "friend request send");
			break;
		case TrackingEvent.TrackingType.FriendshipAccepted:
			DoTrackEvent(ga, "game", "friends", string.Concat("accept-", evt.Arguments["Sender"], "-", evt.Arguments["Recipient"]));
			break;
		case TrackingEvent.TrackingType.FriendshipRejected:
			DoTrackEvent(ga, "game", "friends", string.Concat("decline-", evt.Arguments["Sender"], "-", evt.Arguments["Recipient"]));
			break;
		case TrackingEvent.TrackingType.LoadComplete:
			DoTrackEvent(ga, "duckworld", "Application Loaded", evt.Arguments["Version"].ToString(), 0L, evt.Arguments);
			break;
		case TrackingEvent.TrackingType.BackToMenuButton:
		case TrackingEvent.TrackingType.GameRestart:
		case TrackingEvent.TrackingType.GamePause:
		case TrackingEvent.TrackingType.GameResume:
			DoTrackEvent(ga, evt.Arguments["LocationName"].ToString(), string.Concat(evt.Arguments["GameName"], " ", evt.Type), string.Concat(evt.Arguments["GameName"], " ", evt.Arguments["Difficulty"]));
			break;
		case TrackingEvent.TrackingType.PlayerJournalOpened:
		case TrackingEvent.TrackingType.ViewOpened:
		case TrackingEvent.TrackingType.ViewClosed:
		case TrackingEvent.TrackingType.AvatarCreated:
		case TrackingEvent.TrackingType.AvatarSaved:
		case TrackingEvent.TrackingType.DuckcoinsEarned:
		case TrackingEvent.TrackingType.ChallengeRequested:
		case TrackingEvent.TrackingType.ChallengeCompleted:
		case TrackingEvent.TrackingType.ChallengeRejected:
		case TrackingEvent.TrackingType.MotionComicOpened:
			break;
		}
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
		if (UserProfile.Current.CustomDimensions != null)
		{
			foreach (KeyValuePair<string, string> customDimension in UserProfile.Current.CustomDimensions)
			{
				if (customDimension.Key.Contains("dimension"))
				{
					int result2 = 0;
					if (int.TryParse(customDimension.Key.Replace("dimension", string.Empty), out result2) && !result.GetCustomDimensions().ContainsKey(result2))
					{
						int dimensionNumber = result2;
						string value = customDimension.Value;
						result.SetCustomDimension(dimensionNumber, value);
					}
				}
			}
		}
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

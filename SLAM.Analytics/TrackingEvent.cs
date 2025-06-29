using System.Collections.Generic;
using SLAM.BuildSystem;

namespace SLAM.Analytics;

public class TrackingEvent
{
	public enum TrackingType
	{
		HubOpened,
		PlayerJournalOpened,
		LocationOpened,
		StartViewOpened,
		ViewOpened,
		ViewClosed,
		GameStart,
		GameQuit,
		GameWon,
		GameLost,
		AvatarCreated,
		AvatarSaved,
		DuckcoinsEarned,
		ItemBought,
		FriendshipRequested,
		FriendshipAccepted,
		FriendshipRejected,
		ChallengeRequested,
		ChallengeCompleted,
		ChallengeRejected,
		MotionComicOpened,
		LoadComplete,
		BackToMenuButton,
		GameRestart,
		GamePause,
		GameResume,
		KartBoughtEvent,
		KartCustomizedEvent
	}

	public TrackingType Type;

	private Dictionary<string, object> arguments = new Dictionary<string, object>();

	public Dictionary<string, object> Arguments
	{
		get
		{
			return arguments;
		}
		set
		{
			if (!value.ContainsKey("Version"))
			{
				value.Add("Version", SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString());
			}
			arguments = value;
		}
	}
}

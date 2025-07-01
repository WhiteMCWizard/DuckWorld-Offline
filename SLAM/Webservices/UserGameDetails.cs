using System;
using LitJson;

namespace SLAM.Webservices;

[Serializable]
public class UserGameDetails
{
	[JsonName("id")]
	public int Id;

	[JsonName("is_unlocked")]
	public bool IsUnlocked;

	[JsonName("is_unlocked_sa")]
	public bool IsUnlockedSA;

	[JsonName("has_finished")]
	public bool HasFinished;

	[JsonName("levels")]
	public UserGameProgression[] Progression;

	[JsonName("game")]
	public int GameId;

	public UserGameDetails()
	{
	}

	public UserGameDetails(int gameId, UserGameProgression[] prog)
	{
		GameId = gameId;
		Progression = prog;
	}
}

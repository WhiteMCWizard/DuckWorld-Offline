using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class GameProgressionManager : MonoBehaviour
{
	public bool IsLoaded => GameDetails != null;

	public UserGameDetails[] GameDetails { get; protected set; }

	public bool IsUnlocked(Game game)
	{
		if (!game.PreviousGameId.HasValue)
		{
			return true;
		}
		return GameDetails.Any((UserGameDetails gd) => gd.GameId == game.Id);
	}

	public bool IsUnlocked(int gameId)
	{
		return GameDetails.Any((UserGameDetails gd) => gd.GameId == gameId);
	}

	private void Awake()
	{
		DataStorage.GetProgressionData(delegate(UserGameDetails[] ugd)
		{
			GameDetails = ugd;
		});
	}
}

using System;
using LitJson;
using SLAM.SaveSystem;
using SLAM.Slinq;

namespace SLAM.Webservices;

public static class DataStorage
{
	private static UserGameDetails[] userProgressionInfo;

	private static Location[] locations;

	private static UserProfile[] friends;

	private static WebConfiguration config;

	public static void DeleteAll()
	{
		config = null;
		friends = null;
		locations = null;
		userProgressionInfo = null;
	}

	public static void GetProgressionData(Action<UserGameDetails[]> callback, bool forceRefresh = false)
	{
		if (userProgressionInfo != null && !forceRefresh && callback != null)
		{
			callback(userProgressionInfo);
			return;
		}
		
		// Get progression data from local save instead of API
		var saveData = SaveManager.Instance.GetSaveData();
		userProgressionInfo = saveData != null ? saveData.userGameDetails : new UserGameDetails[0];
		if (callback != null)
		{
			callback(userProgressionInfo);
		}
	}

	public static void GetLocationsData(Action<Location[]> callback)
	{
		if (locations != null)
		{
			callback(locations);
			return;
		}
		locations = Locations.GetLocations();
		callback(locations);
	}

	public static void GetGameById(int gameId, Action<Game> callback)
	{
		GetLocationsData(delegate(Location[] locations)
		{
			Location location = locations.FirstOrDefault((Location l) => l.Games.Any((Game g) => g.Id == gameId));
			Game game = location.GetGame(gameId);
			if (callback != null)
			{
				callback(game);
			}
		});
	}

	public static void GetFriends(Action<UserProfile[]> callback, bool forceRefresh = false)
	{
		if (friends != null && !forceRefresh && callback != null)
		{
			callback(friends);
			return;
		}
	}
}

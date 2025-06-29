using LitJson;

namespace SLAM.Webservices;

public class AdventureData
{
	[JsonName("name")]
	public string Name;

	[JsonName("games")]
	public GameData[] Games;
}

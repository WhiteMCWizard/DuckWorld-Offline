using LitJson;

namespace SLAM.Webservices;

public class Location
{
	[JsonName("games")]
	public Game[] Games { get; set; }

	[JsonName("id")]
	public int Id { get; set; }

	[JsonName("name")]
	public string Name { get; set; }

	[JsonName("description")]
	public string Description { get; set; }

	[JsonName("enabled")]
	public bool Enabled { get; set; }

	[JsonName("is_unlocked")]
	public bool IsUnlocked { get; set; }

	public Location()
	{
	}

	public Location(int id, string name, Game[] games)
	{
		Id = id;
		Name = name;
		Games = games;
	}

	public Game GetGame(int id)
	{
		for (int i = 0; i < Games.Length; i++)
		{
			if (Games[i].Id == id)
			{
				return Games[i];
			}
		}
		return null;
	}

	public static Location GetById(int id, Location[] locations)
	{
		for (int i = 0; i < locations.Length; i++)
		{
			if (locations[i].Id == id)
			{
				return locations[i];
			}
		}
		return null;
	}

	public override string ToString()
	{
		return Name;
	}
}

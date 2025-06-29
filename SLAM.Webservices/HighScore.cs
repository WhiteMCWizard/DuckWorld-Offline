using LitJson;

namespace SLAM.Webservices;

public class HighScore
{
	[JsonName("name")]
	public string Username;

	[JsonName("rank")]
	public int Rank;

	[JsonName("score")]
	public int Score;

	[JsonName("time")]
	public int Time;

	[JsonName("you")]
	public bool You;

	[JsonName("id")]
	public int Id;
}

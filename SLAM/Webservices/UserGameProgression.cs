using LitJson;

namespace SLAM.Webservices;

public class UserGameProgression
{
	[JsonName("difficulty")]
	public int LevelIndex;

	[JsonName("level")]
	public string Level;

	[JsonName("highscore")]
	public int Score;

	[JsonName("besttime")]
	public int Time;

	public UserGameProgression()
	{
	}

	public UserGameProgression(int levelIndex)
	{
		LevelIndex = levelIndex;
	}
}

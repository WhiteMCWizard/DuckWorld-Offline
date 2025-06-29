using LitJson;

namespace SLAM.Webservices;

public class GhostRecording
{
	[JsonName("game")]
	public int GameId;

	[JsonName("level")]
	public string Level;

	[JsonName("coords_url")]
	public string RecordingUrl;

	[JsonName("kart_config")]
	public string KartConfigJson;

	[JsonName("avatar_config")]
	public string AvatarConfigJson;

	[JsonName("time")]
	public int TimeInMilliseconds;
}

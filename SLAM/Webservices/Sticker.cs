using LitJson;

namespace SLAM.Webservices;

public class Sticker
{
	[JsonName("game_name")]
	public string GameName;

	[JsonName("id")]
	public int Id;

	[JsonName("date_created")]
	public string DateCreated;

	[JsonName("date_modified")]
	public string DateModified;

	[JsonName("game")]
	public int GameId;

	[JsonName("stars")]
	public int Stars;

	[JsonName("image")]
	public string Image;
}

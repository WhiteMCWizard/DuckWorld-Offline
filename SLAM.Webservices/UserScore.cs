using LitJson;

namespace SLAM.Webservices;

public class UserScore
{
	[JsonName("id")]
	public int Id;

	[JsonName("date_created")]
	public string DateCreated;

	[JsonName("date_modified")]
	public string DateModified;

	[JsonName("gameuser")]
	public int UserId;

	[JsonName("game")]
	public int GameId;

	[JsonName("score")]
	public int Score;

	[JsonName("sticker")]
	public Sticker Sticker;
}

using LitJson;

namespace SLAM.Webservices;

public class BadgeData
{
	[JsonName("title")]
	public string Name;

	[JsonName("description")]
	public string Description;

	[JsonName("image")]
	public string Image;
}

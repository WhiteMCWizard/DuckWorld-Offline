using LitJson;

namespace SLAM.Webservices;

public class Job
{
	[JsonName("id")]
	public int Id;

	[JsonName("name")]
	public string Name;

	[JsonName("available")]
	public string Available;

	[JsonName("wait")]
	public int Wait;
}

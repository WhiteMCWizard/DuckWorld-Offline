using LitJson;

namespace SLAM.Webservices;

public class AuthToken
{
	[JsonName("token")]
	public string Token;

	[JsonName("session_id")]
	public string SessionID;
}

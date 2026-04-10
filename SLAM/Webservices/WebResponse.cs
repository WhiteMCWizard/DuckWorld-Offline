using LitJson;

namespace SLAM.Webservices;

// Removed: HTTP response processing no longer needed for offline play.
public class WebResponse
{
	public class WebError
	{
		[JsonName("status_code")]
		public int StatusCode;

		[JsonName("detail")]
		public string Detail;
	}

	public int StatusCode;
	public string ReasonPhrase;
	public bool Connected = true;
	public WebError Error => null;
}

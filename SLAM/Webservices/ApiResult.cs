using LitJson;

namespace SLAM.Webservices;

public class ApiResult<T>
{
	[JsonName("status")]
	public ApiResultStatus Status;

	[JsonName("reason")]
	public string Reason;

	[JsonName("result")]
	public T Value;
}

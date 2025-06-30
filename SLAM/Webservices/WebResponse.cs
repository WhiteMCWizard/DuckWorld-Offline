using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;

namespace SLAM.Webservices;

public class WebResponse
{
	public class WebError
	{
		[JsonName("status_code")]
		public int StatusCode;

		[JsonName("detail")]
		public string Detail;
	}

	public string HttpVersion;

	public int StatusCode;

	public string ReasonPhrase;

	public bool Connected = true;

	public WWW Request;

	private WebError _error;

	public WebError Error
	{
		get
		{
			if (_error == null)
			{
				try
				{
					_error = JsonMapper.ToObject<WebError>(Request.text);
				}
				catch (JsonException)
				{
					_error = null;
				}
			}
			return _error;
		}
	}

	public WebResponse(WWW request, string method)
	{
		Request = request;
		if (!request.isDone)
		{
			Request.Dispose();
			StatusCode = 408;
			ReasonPhrase = "REQUEST TIMEOUT";
			return;
		}
		if (request.responseHeaders.ContainsKey("STATUS"))
		{
			string[] array = request.responseHeaders["STATUS"].Split(' ');
			HttpVersion = array[0];
			StatusCode = int.Parse(array[1]);
			ReasonPhrase = string.Join(" ", array, 2, array.Length - 2);
			return;
		}
		if (request.error != null)
		{
			Match match = Regex.Match(request.error, "[0-9]+");
			Match match2 = Regex.Match(request.error, "[a-zA-Z ]+");
			HttpVersion = "1.0";
			StatusCode = ((!match.Success) ? (-1) : int.Parse(match.Value));
			ReasonPhrase = ((!match2.Success) ? request.error : match2.Value.Trim());
			Connected = match.Success && !request.error.Contains("Could not resolve host") && !request.error.Contains("Failed dowloading");
			return;
		}
		HttpVersion = "1.0";
		if (string.IsNullOrEmpty(request.error) && method == "POST")
		{
			StatusCode = 201;
			ReasonPhrase = "CREATED";
		}
		else if (string.IsNullOrEmpty(request.error))
		{
			StatusCode = 200;
			ReasonPhrase = "OK";
		}
		else if (SingletonMonobehaviour<Webservice>.Instance.HasAuthenticationToken())
		{
			StatusCode = 401;
			ReasonPhrase = "Unauthorized";
		}
		else
		{
			StatusCode = 403;
			ReasonPhrase = "Forbidden";
		}
	}
}

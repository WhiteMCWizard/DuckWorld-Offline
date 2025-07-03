using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using SLAM.SaveSystem;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Webservices;

public class Webservice : SingletonMonobehaviour<Webservice>
{
	public class WebserviceErrorEvent
	{
		public WebResponse Response;
	}

	public class LogoutEvent
	{
		public Action<AsyncOperation> LoginLoadedCallback;
	}

	public class TrialEndedEvent
	{
	}

	private int keepaliveInterval = 180;

	private List<WebRequest> requestHistory = new List<WebRequest>();

	public string AuthToken { get; protected set; }

	public string SessionID { get; protected set; }

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		authenticate();
	}

	private void Update()
	{
	}

	protected void authenticate()
	{
		AuthToken = PlayerPrefs.GetString("auth_token", string.Empty);
	}

	public void ReceiveToken(string token, string sessionId = "", bool persistent = true)
	{
		AuthToken = token;
		SessionID = sessionId;
		if (!string.IsNullOrEmpty(token))
		{
			keepaliveInterval = 180;
		}
		if (persistent)
		{
			PlayerPrefs.SetString("auth_token", token);
			PlayerPrefs.SetString("session_id", SessionID);
		}
		else
		{
			PlayerPrefs.DeleteKey("auth_token");
			PlayerPrefs.DeleteKey("session_id");
		}
		if (persistent)
		{
			PlayerPrefs.SetString("auth_token", token);
		}
		else
		{
			PlayerPrefs.DeleteKey("auth_token");
		}
		PlayerPrefs.Save();
	}

	public bool HasAuthenticationToken()
	{
		return !string.IsNullOrEmpty(AuthToken);
	}

	public WebRequest DoRequest<T>(string method, string url, Action<T> callback)
	{
		return DoRequest(method, url, new WWWForm(), callback);
	}

	public WebRequest DoRequest<T>(string method, string url, Dictionary<string, object> data, Action<T> callback)
	{
		WWWForm wWWForm = new WWWForm();
		if (data != null)
		{
			foreach (KeyValuePair<string, object> datum in data)
			{
				if (datum.Value.GetType().IsArray)
				{
					foreach (object item in (Array)datum.Value)
					{
						wWWForm.AddField(datum.Key, item.ToString());
					}
				}
				else if (datum.Value is int)
				{
					wWWForm.AddField(datum.Key, (int)datum.Value);
				}
				else if (datum.Value is string)
				{
					wWWForm.AddField(datum.Key, (string)datum.Value);
				}
				else
				{
					wWWForm.AddField(datum.Key, JsonMapper.ToJson(datum.Value));
				}
			}
		}
		return DoRequest(method, url, wWWForm, callback);
	}

	public WebRequest DoRequest<T>(string method, string url, WWWForm form, Action<T> callback)
	{
		return DoRequest(method, url, WebRequest.CalculateHash(form), form, callback);
	}

	public WebRequest DoRequest<T>(string method, string url, string hash, WWWForm form, Action<T> callback)
	{
		WebRequest webRequest = WebRequest.Start(method, url, hash, form, callback);
		requestHistory.Add(webRequest);
		return webRequest;
	}

	public static void WaitFor(Action callback, params string[] requests)
	{
		WaitFor(callback, new List<string>(requests));
	}

	public static void WaitFor(Action callback, IEnumerable<WebRequest> requests)
	{
		WaitFor(callback, requests.Select((WebRequest r) => r.Url), requests.Select((WebRequest r) => r.Method));
	}

	public static void WaitFor(Action callback, params WebRequest[] requests)
	{
		WaitFor(callback, requests.Select((WebRequest r) => r.Url), requests.Select((WebRequest r) => r.Method));
	}

	public static void WaitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods = null)
	{
		SingletonMonobehaviour<Webservice>.Instance.waitFor(callback, urls, methods);
	}

	private void waitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods)
	{
		StartCoroutine(doWaitFor(callback, urls, methods));
	}

	private IEnumerator doWaitFor(Action callback, IEnumerable<string> urls, IEnumerable<string> methods)
	{
		string[] methds = methods?.ToArray();
		int index = 0;
		foreach (string url in urls)
		{
			WebRequest lastRequestWithThisUrl = requestHistory.LastOrDefault((WebRequest req) => req.Url == url && (methds == null || methds[index] == req.Method));
			if (lastRequestWithThisUrl == null)
			{
				index++;
				Debug.LogWarning("Couldnt find a request to wait for with url: " + url);
				continue;
			}
			while (!lastRequestWithThisUrl.IsDone)
			{
				yield return null;
			}
			index++;
		}
		callback?.Invoke();
	}
}

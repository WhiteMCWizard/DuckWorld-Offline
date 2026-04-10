using UnityEngine;

namespace SLAM.Webservices;

public class Webservice : SingletonMonobehaviour<Webservice>
{
	// These event classes are still used as game events throughout the codebase
	public class WebserviceErrorEvent
	{
		public WebResponse Response;
	}

	public class LogoutEvent
	{
		public System.Action<AsyncOperation> LoginLoadedCallback;
	}

	public class TrialEndedEvent
	{
	}

	protected override void Awake()
	{
		base.Awake();
		Object.DontDestroyOnLoad(gameObject);
	}
}

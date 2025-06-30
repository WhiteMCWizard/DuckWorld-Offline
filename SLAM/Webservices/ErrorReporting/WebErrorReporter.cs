using System.Collections.Generic;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.SaveSystem;
using UnityEngine;

namespace SLAM.Webservices.ErrorReporting;

public class WebErrorReporter : MonoBehaviour
{
	private bool isHandlingError;

	private List<int> panicErrorCodes = new List<int> { 401, 408, 418 };

	private void OnEnable()
	{
		GameEvents.Subscribe<Webservice.WebserviceErrorEvent>(onWebserviceError);
		GameEvents.Subscribe<Webservice.LogoutEvent>(onLogoutEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<Webservice.WebserviceErrorEvent>(onWebserviceError);
		GameEvents.Unsubscribe<Webservice.LogoutEvent>(onLogoutEvent);
	}

	private void onWebserviceError(Webservice.WebserviceErrorEvent evt)
	{
		Debug.LogErrorFormat("Webservice error occurred \n url:{0} \n errorcode:{1} \n error: {2} \n text: {3} \n \n wwwerror: {4} connected: {5}", evt.Response.Request.url, evt.Response.StatusCode, evt.Response.ReasonPhrase, evt.Response.Request.text, evt.Response.Request.error, evt.Response.Connected);
		if ((evt.Response.StatusCode < 500 && !panicErrorCodes.Contains(evt.Response.StatusCode) && evt.Response.StatusCode != -1) || (panicErrorCodes.Contains(evt.Response.StatusCode) && Application.loadedLevelName == "Login") || isHandlingError)
		{
			return;
		}
		isHandlingError = true;
		if (Application.loadedLevelName != "Hub" && evt.Response.StatusCode == 418)
		{
			SceneManager.Load("Hub", delegate
			{
				GameEvents.Invoke(new Webservice.TrialEndedEvent());
			});
		}
		else if (Application.loadedLevelName != "Login")
		{
			Webservice.LogoutEvent logoutEvent = new Webservice.LogoutEvent();
			logoutEvent.LoginLoadedCallback = delegate
			{
				isHandlingError = false;
				onWebserviceError(evt);
			};
			GameEvents.Invoke(logoutEvent);
		}
		else
		{
			string key = $"ERROR_{evt.Response.StatusCode}_TITLE";
			string key2 = $"ERROR_{evt.Response.StatusCode}_TEXT";
			string title = ((!Localization.Exists(key)) ? Localization.Get("ERROR_GENERIC_TITLE") : Localization.Get(key));
			string message = ((!Localization.Exists(key2)) ? Localization.Get("ERROR_GENERIC_TEXT") : Localization.Get(key2));
			GameEvents.Invoke(new PopupEvent(title, message, Localization.Get("UI_OK"), null));
			isHandlingError = false;
		}
	}

	private void onLogoutEvent(Webservice.LogoutEvent evt)
	{
		ApiClient.Logout();
		UserProfile.UnsetCurrentProfileData();
		AvatarSystem.UnsetPlayerConfiguration();
		DataStorage.DeleteAll();
		SingletonMonobehaviour<Webservice>.Instance.ReceiveToken(null, string.Empty);
		SceneManager.Load("Login", evt.LoginLoadedCallback);
	}
}

using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.SaveSystem;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse;

public class LoginController : ViewController
{
	[SerializeField]
	private View[] views;

	private void Awake()
	{
		AddViews(views);
	}

	protected override void Start()
	{
		base.Start();
		OpenView<LoginView>().DemoButtonEnabled = false;
		// Version checking and update system - removed as not currently needed
		// May be re-implemented in the future
		//
		// UpdateSystem.HasLatestVersion(delegate(bool hasLatest)
		// {
		//     if (!hasLatest)
		//     {
		//         UpdateSystem.UpdateToLatestVersion();
		//     }
		// });
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<Webservice.WebserviceErrorEvent>(onError);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<Webservice.WebserviceErrorEvent>(onError);
	}

	private void onError(Webservice.WebserviceErrorEvent evt)
	{
		WebResponse.WebError error = evt.Response.Error;
		GetView<LoginView>().ShowFeedback("UI_LOGIN_CONNECTION_PROBLEMS");
		if (error != null && error.StatusCode == 401)
		{
			if (error.Detail.Contains("User is banned"))
			{
				GetView<LoginView>().ShowFeedback("UI_LOGIN_BANNED");
			}
			else if (error.Detail.Contains("Invalid or not existing license"))
			{
				GetView<LoginView>().ShowFeedback("UI_LOGIN_LICENSE_EXPIRED");
			}
		}
	}

	public void Login(string username, string password)
	{
		ApiClient.Authenticate(username, password, delegate(bool result)
		{
			if (result)
			{
				UserProfile.GetCurrentProfileData(delegate(UserProfile profile)
				{
					if (profile != null)
					{
						PlayerPrefs.SetString("login_username", username);
						SceneManager.Load("FirstResponse");
					}
				});
			}
			else
			{
				GetView<LoginView>().LoginButtonEnabled = true;
				GetView<LoginView>().ShowFeedback("UI_LOGIN_INVALID_CREDENTIALS");
			}
		});
	}

	public void FreePlay()
	{
		ApiClient.GetFreeUserToken(delegate
		{
			UserProfile.GetCurrentProfileData(delegate
			{
				GetView<LoginView>().DemoButtonEnabled = true;
				SceneManager.Load("FirstResponse");
			});
		});
	}

	public void TipParentClicked()
	{
		if (!GetView<TipParentView>().IsOpen)
		{
			OpenView<TipParentView>();
		}
	}
}

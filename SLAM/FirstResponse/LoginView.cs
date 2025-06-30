using SLAM.Engine;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse;

public class LoginView : View
{
	[SerializeField]
	private UIInput usernameInput;

	[SerializeField]
	private UIInput passwordInput;

	[SerializeField]
	private UILabel feedbackLabel;

	[SerializeField]
	private UIButton loginButton;

	[SerializeField]
	private UIButton demoButton;

	public bool LoginButtonEnabled
	{
		get
		{
			return loginButton.isEnabled;
		}
		set
		{
			loginButton.isEnabled = value;
		}
	}

	public bool DemoButtonEnabled
	{
		get
		{
			return demoButton.isEnabled;
		}
		set
		{
			demoButton.isEnabled = value;
		}
	}

	protected override void Start()
	{
		base.Start();
		usernameInput.value = PlayerPrefs.GetString("login_username", string.Empty);
	}

	public void ShowFeedback(string localeKey)
	{
		if (!string.IsNullOrEmpty(localeKey))
		{
			passwordInput.value = string.Empty;
			feedbackLabel.text = StringFormatter.GetLocalizationFormatted(localeKey);
		}
		else
		{
			feedbackLabel.text = string.Empty;
		}
	}

	public void OnLoginClicked()
	{
		ShowFeedback(string.Empty);
		if (isInputValid())
		{
			loginButton.isEnabled = false;
			Controller<LoginController>().Login(usernameInput.value, passwordInput.value);
		}
		else
		{
			ShowFeedback("UI_LOGIN_NO_EMPTY_FIELDS");
		}
	}

	public void OnFreePlayClicked()
	{
		demoButton.isEnabled = false;
		Controller<LoginController>().FreePlay();
	}

	public void OnRegisterClicked()
	{
		ApiClient.OpenRegisterPage();
	}

	public void OnForgotPasswordClicked()
	{
		ApiClient.OpenForgotPasswordPage();
	}

	public void OnHelpClicked()
	{
		ApiClient.OpenHelpPage();
	}

	public void OnTipParentClicked()
	{
		Controller<LoginController>().TipParentClicked();
	}

	public void OnQuitClicked()
	{
		Application.Quit();
	}

	private bool isInputValid()
	{
		return !string.IsNullOrEmpty(usernameInput.value) && !string.IsNullOrEmpty(passwordInput.value);
	}
}

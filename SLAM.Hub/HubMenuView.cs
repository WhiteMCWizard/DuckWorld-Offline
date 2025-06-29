using SLAM.Engine;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Hub;

public class HubMenuView : View
{
	public void OnHelpClicked()
	{
		ApiClient.OpenHelpPage();
	}

	public void OnOptionsClicked()
	{
		SmartphoneController smartphoneController = Object.FindObjectOfType<SmartphoneController>();
		OnCloseClicked();
		smartphoneController.Show();
		smartphoneController.OpenApp<SettingsApp>();
	}

	public void OnLogoutClicked()
	{
		Controller<HubController>().LogoutCurrentUser();
	}

	public void OnQuitClicked()
	{
		Application.Quit();
	}

	public void OnCloseClicked()
	{
		Controller<HubController>().CloseSettingsWindow();
	}
}

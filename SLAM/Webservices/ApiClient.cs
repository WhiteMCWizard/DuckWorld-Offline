using SLAM.SaveSystem;
using UnityEngine;

namespace SLAM.Webservices;

public static class ApiClient
{
	public static int UserId => UserProfile.Current != null ? UserProfile.Current.Id : 0;

	public static void OpenHelpPage()
	{
		Application.OpenURL("https://github.com/WhiteMCWizard/DuckWorld-Offline/issues");
	}

	// Stubs for UI buttons that still reference these methods
	public static void OpenRegisterPage() { }
	public static void OpenPropositionPage(int gameId) { }
	public static void OpenForgotPasswordPage() { }
}

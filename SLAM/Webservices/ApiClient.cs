using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using SLAM.Achievements;
using SLAM.Avatar;
using SLAM.Kart;
using SLAM.KartRacing;
using SLAM.SaveSystem;
using UnityEngine;

namespace SLAM.Webservices;

public static class ApiClient
{
	public static int UserId => UserProfile.Current.Id;

	public static string API_URL { get; private set; }

	static ApiClient()
	{
		API_URL = "http://127.0.0.1:8000/api/1.0";
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "--localhost", "http://127.0.0.1:8000/api/1.0" },
			{ "--test", "https://test.duckworld.com/api/1.0" },
			{ "--staging", "https://staging-new.duckworld.com/api/1.0" },
			{ "--production", "https://www.duckworld.com/api/1.0" }
		};
		IEnumerator enumerator = Environment.GetCommandLineArgs().GetEnumerator();
		while (enumerator.MoveNext())
		{
			string text = (string)enumerator.Current;
			if (dictionary.TryGetValue(text, out var value))
			{
				API_URL = value;
			}
			if (text == "--host" && enumerator.MoveNext())
			{
				API_URL = (string)enumerator.Current;
			}
		}
	}


	public static void OpenRegisterPage()
	{
	}

	public static void OpenPropositionPage(int gameId)
	{
	}

	public static void OpenForgotPasswordPage()
	{
	}

	public static void OpenHelpPage()
	{
		Application.OpenURL("https://github.com/WhiteMCWizard/DuckWorld-Offline/issues");
	}

	private static Action<WebResponse> validateResponseStatus(int requiredStatus, Action<bool> callback)
	{
		return delegate(WebResponse response)
		{
			if (callback != null)
			{
				callback(response.StatusCode == requiredStatus);
			}
		};
	}
}

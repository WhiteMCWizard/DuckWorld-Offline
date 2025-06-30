using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SLAM.Analytics;
using SLAM.Avatar;
using SLAM.BuildSystem;
using SLAM.SaveSystem;
using SLAM.Smartphone;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse;

public class FirstResponse : MonoBehaviour
{
	private IEnumerator Start()
	{
		checkForExistingProcess();
		SettingsView.InitializeSettings();
		SaveManager.Instance.Load(
			new BinarySaveDataProvider(
				System.IO.Path.Combine(Application.persistentDataPath, "savegame.dat")
			)
		);
		UserProfile.GetCurrentProfileData(gotUserProfile);
		yield return null;
	}

	private void gotUserProfile(UserProfile profile)
	{
		AvatarSystem.LoadPlayerConfiguration();
		SceneManager.Load("Hub", delegate
		{
			trackLoadComplete();
		});
	}

	[DllImport("user32.dll")]
	private static extern bool SetForegroundWindow(IntPtr hWnd);

	private void checkForExistingProcess()
	{
		Process currentProcess = Process.GetCurrentProcess();
		Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
		if (processesByName.Length <= 1)
		{
			return;
		}
		foreach (Process process in processesByName)
		{
			if (process.Id != currentProcess.Id)
			{
				SetForegroundWindow(process.MainWindowHandle);
				Application.Quit();
			}
		}
	}

	private static void trackLoadComplete()
	{
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.LoadComplete;
		trackingEvent.Arguments = new Dictionary<string, object>
		{
			{ "UserLoggedIn", "1" },
			{
				"LoadingTime",
				Time.realtimeSinceStartup
			}
		};
		GameEvents.Invoke(trackingEvent);
	}
}

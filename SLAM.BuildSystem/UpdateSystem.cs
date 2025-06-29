using System;
using System.Text.RegularExpressions;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.BuildSystem;

public static class UpdateSystem
{
	public static void HasLatestVersion(Action<bool> callback)
	{
		DataStorage.GetWebConfiguration(delegate(WebConfiguration config)
		{
			if (config == null)
			{
				callback(obj: true);
			}
			else
			{
				bool flag = Regex.IsMatch(SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString(), config.Version);
				bool flag2 = Environment.GetCommandLineArgs().Any((string cmd) => cmd == "-force-update");
				bool flag3 = Environment.GetCommandLineArgs().Any((string cmd) => cmd == "-skip-update");
				bool flag4 = (!flag && !flag3) || flag2;
				Debug.LogFormat("Version check: local version: {0}, online version: {1}, versionIsMatched: {2}, forceUpdate: {3}, skipUpdate: {4}, shouldUpdate: {5}", SceneDataLibrary.GetSceneDataLibrary().GameVersion, config.Version, flag, flag2, flag3, flag4);
				if (callback != null)
				{
					callback(!flag4);
				}
			}
		}, forceRefresh: true);
	}

	public static void UpdateToLatestVersion()
	{
		DataStorage.GetWebConfiguration(delegate(WebConfiguration config)
		{
			GameEvents.Invoke(new PopupEvent(Localization.Get("ERROR_UPDATE_TITLE"), string.Format(Localization.Get("ERROR_UPDATE_BODY"), SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString(), config.Version), Localization.Get("UI_OK"), delegate
			{
				SceneManager.Load("UpdateGame");
			}));
		});
	}
}

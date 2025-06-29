using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using SLAM.Engine;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.FirstResponse;

public class UpdateController : MonoBehaviour
{
	[SerializeField]
	private UILabel statusLabel;

	[SerializeField]
	private UIProgressBar progressbar;

	private void Start()
	{
		DataStorage.GetWebConfiguration(delegate(WebConfiguration config)
		{
			StartCoroutine(doUpdateSequence(config));
		});
	}

	private IEnumerator doUpdateSequence(WebConfiguration config)
	{
		string installerUrl = config.WindowsInstallerUrl;
		string installerChecksum = config.WindowsInstallerChecksum;
		yield return null;
		setStatus(Localization.Get("UI_INSTALL_DOWNLOADING_FILES"));
		WWW www = new WWW(installerUrl);
		while (!www.isDone)
		{
			progressbar.value = www.progress;
			yield return null;
		}
		progressbar.value = www.progress;
		setStatus(Localization.Get("UI_INSTALL_DONE_DOWNLOADING"));
		yield return null;
		string installerFile = installerUrl.Split('/').Last();
		string installerPath = Path.Combine(path2: (!(installerFile != string.Empty)) ? "installer.exe" : installerFile, path1: Application.temporaryCachePath);
		try
		{
			if (!Directory.Exists(Application.temporaryCachePath))
			{
				Directory.CreateDirectory(Application.temporaryCachePath);
			}
			UnityEngine.Debug.LogFormat("Downloaded {0} to {1}", www.url, installerPath);
			File.WriteAllBytes(installerPath, www.bytes);
			if (installerChecksum == WebRequest.MD5(www.bytes))
			{
				setStatus(Localization.Get("UI_INSTALL_START"));
				Process.Start(installerPath);
				Application.Quit();
				yield break;
			}
			throw new Exception(Localization.Get("ERROR_DOWNLOAD_CORRUPTED"));
		}
		catch (Exception ex)
		{
			WebConfiguration config2 = default(WebConfiguration);
			GameEvents.Invoke(new PopupEvent(Localization.Get("UI_SOMETHING_WENT_WRONG"), Localization.Get("UI_ERROR_OCCURED") + ex.Message, Localization.Get("UI_OK"), delegate
			{
				Application.OpenURL(config2.DownloadPageUrl);
				Application.Quit();
			}));
		}
	}

	private void setStatus(string status)
	{
		statusLabel.text = status;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.BuildSystem;

public class AssetBundleManager
{
	public class AssetLoadRequest
	{
		public string LevelName;

		public float Progress;

		public bool IsDone;
	}

	private Dictionary<string, AssetBundle> loadedAssetBundles = new Dictionary<string, AssetBundle>();

	private Dictionary<string, AssetLoadRequest> loadingRequests = new Dictionary<string, AssetLoadRequest>();

	public bool HasBundleForLevel(string levelName)
	{
		return loadedAssetBundles.ContainsKey(levelName);
	}

	public AssetLoadRequest LoadBundleForLevel(string levelName)
	{
		if (loadingRequests.ContainsKey(levelName))
		{
			return loadingRequests[levelName];
		}
		AssetLoadRequest assetLoadRequest = new AssetLoadRequest();
		assetLoadRequest.LevelName = levelName;
		AssetLoadRequest assetLoadRequest2 = assetLoadRequest;
		StaticCoroutine.Start(loadBundle(levelName, assetLoadRequest2));
		return assetLoadRequest2;
	}

	private IEnumerator loadBundle(string levelName, AssetLoadRequest loadRequest)
	{
		loadingRequests.Add(levelName, loadRequest);
		string url = getUrlForLevel(levelName);
		SceneDataLibrary.LevelAssetVersion levelVersionData = SceneDataLibrary.GetSceneDataLibrary().GetVersionData(levelName);
		WWW www = WWW.LoadFromCacheOrDownload(url, levelVersionData.Version, (uint)levelVersionData.CRC);
		while (!www.isDone)
		{
			loadRequest.Progress = www.progress;
			yield return null;
		}
		yield return null;
		if (www.error != null)
		{
			Debug.LogError("Couldnt download " + www.url + " error: " + www.error);
		}
		loadRequest.Progress = 1f;
		loadRequest.IsDone = true;
		loadedAssetBundles.Add(levelName, www.assetBundle);
		loadingRequests.Remove(levelName);
	}

	private string getUrlForLevel(string levelName)
	{
		return Application.dataPath + "/" + levelName + ".unity?version=" + SceneDataLibrary.GetSceneDataLibrary().GameVersion.ToString();
	}
}

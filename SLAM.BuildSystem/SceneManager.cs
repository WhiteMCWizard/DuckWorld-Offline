using System;
using System.Collections;
using UnityEngine;

namespace SLAM.BuildSystem;

public class SceneManager : MonoBehaviour
{
	private const float progressionDelta = 1f;

	private static string scene = string.Empty;

	private static bool preloading;

	private static Action<AsyncOperation> callback;

	private static UIProgressBar progressbar;

	private float targetProgress;

	private AsyncOperation loadingRequest;

	private string _scene;

	private bool _preloading;

	private Action<AsyncOperation> _callback;

	private UIProgressBar _progressbar;

	private void Awake()
	{
		_scene = scene;
		_preloading = preloading;
		_callback = callback;
		_progressbar = progressbar;
	}

	private IEnumerator Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_progressbar.value = 0f;
		loadingRequest = null;
		yield return null;
		if (!_preloading)
		{
			yield return StartCoroutine(loadRequestedScene());
		}
		if (_callback != null)
		{
			_callback(loadingRequest);
		}
		if (_preloading)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}
		UnityEngine.Object.FindObjectOfType<LoadingScreenManager>().FadeLoadingScreenOut(delegate
		{
			UnityEngine.Object.Destroy(base.gameObject);
		});
	}

	private IEnumerator loadRequestedScene()
	{
		loadingRequest = Application.LoadLevelAsync(_scene);
		loadingRequest.allowSceneActivation = false;
		while (!Mathf.Approximately(_progressbar.value, 0.9f))
		{
			targetProgress = Mathf.Lerp(0f, 1f, loadingRequest.progress);
			_progressbar.value = Mathf.MoveTowards(_progressbar.value, targetProgress, 1f * Time.deltaTime);
			yield return null;
		}
		_progressbar.value = 1f;
		loadingRequest.allowSceneActivation = true;
		while (!loadingRequest.isDone)
		{
			yield return null;
		}
	}

	public static Texture2D GetLoadingScreenTextureForLevel(string levelName)
	{
		SceneDataLibrary.LevelAssetVersion versionData = SceneDataLibrary.GetSceneDataLibrary().GetVersionData(levelName);
		string path = ((versionData != null && !(versionData.LoadingScreenName == "none")) ? versionData.LoadingScreenName : "hub_loading_screen");
		return Resources.Load<Texture2D>(path);
	}

	private static void LoadLevel(string scn, Action<AsyncOperation> cb = null)
	{
		LoadingScreenManager loadingScreenManager = UnityEngine.Object.FindObjectOfType<LoadingScreenManager>();
		scene = scn;
		callback = cb;
		progressbar = loadingScreenManager.ProgressBar;
		progressbar.value = 0f;
		if (preloading)
		{
			Application.LoadLevelAdditiveAsync("SceneLoader");
			return;
		}
		loadingScreenManager.SetTexture(GetLoadingScreenTextureForLevel(scene));
		loadingScreenManager.FadeLoadingScreenIn(delegate
		{
			Application.LoadLevelAsync("SceneLoader");
		});
	}

	public static void Load(string scn, Action<AsyncOperation> cb = null)
	{
		preloading = false;
		LoadLevel(scn, cb);
	}

	public static void Preload(string scn, Action<AsyncOperation> cb = null)
	{
	}
}

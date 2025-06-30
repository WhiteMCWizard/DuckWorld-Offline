using System;
using System.Collections;
using SLAM.Shared;
using UnityEngine;

namespace SLAM.BuildSystem;

public class LoadingScreenManager : MonoBehaviour
{
	[SerializeField]
	private UIPanel panel;

	[SerializeField]
	private UITexture uiTexture;

	[SerializeField]
	private UIProgressBar progressBar;

	[SerializeField]
	private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private float fadeDuration = 0.5f;

	public UIProgressBar ProgressBar => progressBar;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnLevelWasLoaded()
	{
	}

	public void SetTexture(Texture2D texture)
	{
		panel.gameObject.SetActive(value: true);
		uiTexture.mainTexture = texture;
	}

	public void FadeLoadingScreenIn(Action callback = null)
	{
		StartCoroutine(fadeIn(callback));
	}

	public void FadeLoadingScreenOut(Action callback = null)
	{
		StartCoroutine(fadeOut(callback));
	}

	private IEnumerator fadeIn(Action callback = null)
	{
		progressBar.value = 0.01f;
		float time = 0f;
		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			float fadeInValue = fadeCurve.Evaluate(time / fadeDuration);
			panel.alpha = fadeInValue;
			SingletonMonobehaviour<AudioManager>.Instance.GlobalVolume = 1f - fadeInValue;
			yield return null;
		}
		callback?.Invoke();
	}

	private IEnumerator fadeOut(Action callback = null)
	{
		float time = 0f;
		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			panel.alpha = fadeCurve.Evaluate(1f - time / fadeDuration);
			yield return null;
		}
		callback?.Invoke();
	}
}

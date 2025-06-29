using System.Collections;
using UnityEngine;

namespace SLAM.Engine.AspectRatioAdjuster;

public abstract class AspectRatioAdjuster : MonoBehaviour
{
	private const float DESIGN_WIDTH = 1280f;

	private const float DESIGN_HEIGHT = 720f;

	private const float MIN_WIDTH = 1024f;

	private const float MIN_HEIGHT = 768f;

	private const float DESIGN_ASPECTRATIO = 1.7777778f;

	private const float MIN_ASPECTRATIO = 1.3333334f;

	private IEnumerator Start()
	{
		int oldWidth = 0;
		int oldHeight = 0;
		bool oldFullscreen = Screen.fullScreen;
		while (true)
		{
			yield return null;
			if (oldWidth != Screen.width || oldHeight != Screen.height || oldFullscreen != Screen.fullScreen)
			{
				oldWidth = Screen.width;
				oldHeight = Screen.height;
				oldFullscreen = Screen.fullScreen;
				doAdjust();
			}
		}
	}

	private void OnLevelWasLoaded(int index)
	{
		doAdjust();
	}

	[ContextMenu("Force adjusting")]
	private void doAdjust()
	{
		float num = (float)Screen.width / (float)Screen.height;
		Adjust(Mathf.Clamp01((num - 1.3333334f) / 0.44444442f));
	}

	protected abstract void Adjust(float aspectRatioFactor);
}

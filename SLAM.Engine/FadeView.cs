using UnityEngine;

namespace SLAM.Engine;

public class FadeView : View
{
	[SerializeField]
	private TweenColor fadeTween;

	public void SetColors(Color from, Color to)
	{
		if (fadeTween != null)
		{
			fadeTween.from = from;
			fadeTween.to = to;
		}
	}

	public void SetDuration(float duration, AnimationCurve curve)
	{
		if (fadeTween != null)
		{
			fadeTween.duration = duration;
			fadeTween.animationCurve = curve;
		}
	}
}

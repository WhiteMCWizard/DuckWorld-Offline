using UnityEngine;

public static class UIExtentions
{
	public static float GetLongestAnimationTime(UITweener[] animations)
	{
		float num = 0f;
		for (int i = 0; i < animations.Length; i++)
		{
			animations[i].PlayForward();
			float animationDuration = GetAnimationDuration(animations[i]);
			num = Mathf.Max(num, animationDuration);
		}
		return num;
	}

	public static float GetAnimationDuration(UITweener animation)
	{
		return animation.delay + animation.duration;
	}
}

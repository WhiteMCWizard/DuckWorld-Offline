using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("GUITexture", "Fade Texture", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class FadeTexture : CinemaGlobalAction
{
	public GUITexture target;

	public Color tint = Color.grey;

	private void Awake()
	{
		if (target != null)
		{
			target.enabled = false;
			target.color = Color.clear;
		}
	}

	public override void Trigger()
	{
		if (target != null)
		{
			target.enabled = true;
			target.color = Color.clear;
		}
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		if (target != null)
		{
			float num = time / base.Duration;
			if (num <= 0.25f)
			{
				FadeToColor(Color.clear, tint, num / 0.25f);
			}
			else if (num >= 0.75f)
			{
				FadeToColor(tint, Color.clear, (num - 0.75f) / 0.25f);
			}
		}
	}

	public override void SetTime(float time, float deltaTime)
	{
		if (target != null)
		{
			target.enabled = true;
			if (time >= 0f && time <= base.Duration)
			{
				UpdateTime(time, deltaTime);
			}
			else if (target.enabled)
			{
				target.enabled = false;
			}
		}
	}

	public override void End()
	{
		if (target != null)
		{
			target.enabled = false;
		}
	}

	public override void ReverseEnd()
	{
		Trigger();
	}

	public override void Stop()
	{
		if (target != null)
		{
			target.enabled = false;
		}
	}

	private void FadeToColor(Color from, Color to, float transition)
	{
		target.color = Color.Lerp(from, to, transition);
	}
}

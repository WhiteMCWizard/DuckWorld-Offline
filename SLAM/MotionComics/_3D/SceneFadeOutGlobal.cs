using CinemaDirector;
using UnityEngine;

namespace SLAM.MotionComics._3D;

[CutsceneItem("SLAM", "Fade Scene Out", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class SceneFadeOutGlobal : CinemaGlobalAction
{
	[SerializeField]
	private Color color = Color.black;

	[SerializeField]
	private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 2f), new Keyframe(1f, 1f, 0f, 0f));

	private SceneFadeView fade;

	private void Awake()
	{
		fade = Object.FindObjectOfType<SceneFadeView>();
		FadeToColor(color, Color.clear, 0f);
		SetState(state: false);
	}

	public override void Trigger()
	{
		SetState(state: true);
		FadeToColor(Color.clear, color, 0f);
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		float time2 = time / base.Duration;
		FadeToColor(Color.clear, color, curve.Evaluate(time2));
	}

	public override void SetTime(float time, float deltaTime)
	{
		if (time >= 0f && time <= base.Duration)
		{
			SetState(state: true);
			UpdateTime(time, deltaTime);
		}
	}

	public override void End()
	{
	}

	public override void ReverseEnd()
	{
		SetState(state: true);
		FadeToColor(Color.clear, color, 1f);
	}

	public override void Stop()
	{
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Duration = 0.7f;
	}

	private void FadeToColor(Color from, Color to, float transition)
	{
		if (fade != null)
		{
			fade.SetColor(Color.Lerp(from, to, transition));
		}
	}

	private void SetState(bool state)
	{
		if (fade != null)
		{
			fade.SetState(state);
		}
	}
}

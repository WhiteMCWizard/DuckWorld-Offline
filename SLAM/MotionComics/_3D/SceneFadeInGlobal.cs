using CinemaDirector;
using UnityEngine;

namespace SLAM.MotionComics._3D;

[CutsceneItem("SLAM", "Fade Scene In", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class SceneFadeInGlobal : CinemaGlobalAction
{
	[SerializeField]
	private Color color = Color.black;

	[SerializeField]
	private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 1f, 2f, 0f));

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
		FadeToColor(color, Color.clear, 0f);
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		float time2 = time / base.Duration;
		FadeToColor(color, Color.clear, curve.Evaluate(time2));
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
		SetState(state: false);
	}

	public override void ReverseEnd()
	{
		SetState(state: true);
		FadeToColor(color, Color.clear, 1f);
	}

	public override void Stop()
	{
		SetState(state: false);
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

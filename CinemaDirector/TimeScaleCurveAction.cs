using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Time", "Time Scale Curve", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class TimeScaleCurveAction : CinemaGlobalAction, IRevertable
{
	public AnimationCurve Curve;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private float previousScale;

	public RevertMode EditorRevertMode
	{
		get
		{
			return editorRevertMode;
		}
		set
		{
			editorRevertMode = value;
		}
	}

	public RevertMode RuntimeRevertMode
	{
		get
		{
			return runtimeRevertMode;
		}
		set
		{
			runtimeRevertMode = value;
		}
	}

	public RevertInfo[] CacheState()
	{
		return new RevertInfo[1]
		{
			new RevertInfo(this, typeof(Time), "timeScale", Time.timeScale)
		};
	}

	public override void Trigger()
	{
		previousScale = Time.timeScale;
	}

	public override void UpdateTime(float time, float deltaTime)
	{
		if (Curve != null)
		{
			Time.timeScale = Curve.Evaluate(time);
		}
	}

	public override void End()
	{
	}

	public override void ReverseTrigger()
	{
		Time.timeScale = previousScale;
	}
}

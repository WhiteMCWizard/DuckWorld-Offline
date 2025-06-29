using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Time", "Set Time Scale", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class SetTimeScaleEvent : CinemaGlobalEvent, IRevertable
{
	public float TimeScale = 1f;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private float previousTimescale = 1f;

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
		previousTimescale = Time.timeScale;
		Time.timeScale = TimeScale;
	}

	public override void Reverse()
	{
		Time.timeScale = previousTimescale;
	}
}

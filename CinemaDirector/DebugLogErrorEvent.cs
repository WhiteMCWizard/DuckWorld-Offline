using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Debug", "Log Error", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class DebugLogErrorEvent : CinemaGlobalEvent
{
	public string message = "Error Message";

	public override void Trigger()
	{
		Debug.LogError(message);
	}

	public override void Reverse()
	{
		Debug.LogError($"Reverse: {message}");
	}
}

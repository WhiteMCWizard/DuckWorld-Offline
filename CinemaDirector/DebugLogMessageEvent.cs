using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Debug", "Log Message", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class DebugLogMessageEvent : CinemaGlobalEvent
{
	public string message = "Log Message";

	public override void Trigger()
	{
		Debug.Log(message);
	}

	public override void Reverse()
	{
		Debug.Log($"Reverse: {message}");
	}
}

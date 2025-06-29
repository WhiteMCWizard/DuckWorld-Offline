using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Utility", "Load Level", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class LoadLevelEvent : CinemaGlobalEvent
{
	public int Level;

	public override void Trigger()
	{
		if (Application.isPlaying)
		{
			Application.LoadLevel(Level);
		}
	}
}

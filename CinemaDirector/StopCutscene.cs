namespace CinemaDirector;

[CutsceneItem("Cutscene", "Stop Cutscene", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class StopCutscene : CinemaGlobalEvent
{
	public Cutscene cutscene;

	public override void Trigger()
	{
		if (cutscene != null)
		{
			cutscene.Stop();
		}
	}
}

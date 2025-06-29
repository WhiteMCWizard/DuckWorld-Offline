namespace CinemaDirector;

[CutsceneItem("Cutscene", "Pause Cutscene", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class PauseCutscene : CinemaGlobalEvent
{
	public Cutscene cutscene;

	public override void Trigger()
	{
		if (cutscene != null)
		{
			cutscene.Pause();
		}
	}
}

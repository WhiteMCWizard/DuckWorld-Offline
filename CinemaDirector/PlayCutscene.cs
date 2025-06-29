namespace CinemaDirector;

[CutsceneItem("Cutscene", "Play Cutscene", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class PlayCutscene : CinemaGlobalEvent
{
	public Cutscene cutscene;

	public override void Trigger()
	{
		if (cutscene != null)
		{
			cutscene.Play();
		}
	}
}

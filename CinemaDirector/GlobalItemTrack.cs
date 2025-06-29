namespace CinemaDirector;

[TimelineTrack("Global Track", TimelineTrackGenre.GlobalTrack, new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class GlobalItemTrack : TimelineTrack
{
	public CinemaGlobalEvent[] Events => GetComponentsInChildren<CinemaGlobalEvent>();

	public CinemaGlobalAction[] Actions => GetComponentsInChildren<CinemaGlobalAction>();

	public override TimelineItem[] TimelineItems
	{
		get
		{
			CinemaGlobalEvent[] events = Events;
			CinemaGlobalAction[] actions = Actions;
			TimelineItem[] array = new TimelineItem[events.Length + actions.Length];
			for (int i = 0; i < events.Length; i++)
			{
				array[i] = events[i];
			}
			for (int j = 0; j < actions.Length; j++)
			{
				array[j + events.Length] = actions[j];
			}
			return array;
		}
	}
}

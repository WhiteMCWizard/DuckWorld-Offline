using UnityEngine;

namespace CinemaDirector;

[TimelineTrack("Curve Track", TimelineTrackGenre.MultiActorTrack, new CutsceneItemGenre[] { CutsceneItemGenre.MultiActorCurveClipItem })]
public class MultiCurveTrack : TimelineTrack, IActorTrack
{
	public override TimelineItem[] TimelineItems => GetComponentsInChildren<CinemaMultiActorCurveClip>();

	public Transform Actor
	{
		get
		{
			ActorTrackGroup component = base.transform.parent.GetComponent<ActorTrackGroup>();
			if (component == null)
			{
				return null;
			}
			return component.Actor;
		}
	}

	public override void Initialize()
	{
		TimelineItem[] timelineItems = TimelineItems;
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaMultiActorCurveClip cinemaMultiActorCurveClip = (CinemaMultiActorCurveClip)timelineItems[i];
			cinemaMultiActorCurveClip.Initialize();
		}
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		elapsedTime = time;
		TimelineItem[] timelineItems = TimelineItems;
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaMultiActorCurveClip cinemaMultiActorCurveClip = (CinemaMultiActorCurveClip)timelineItems[i];
			cinemaMultiActorCurveClip.SampleTime(time);
		}
	}

	public override void Stop()
	{
		TimelineItem[] timelineItems = TimelineItems;
		for (int i = 0; i < timelineItems.Length; i++)
		{
			CinemaMultiActorCurveClip cinemaMultiActorCurveClip = (CinemaMultiActorCurveClip)timelineItems[i];
			cinemaMultiActorCurveClip.Revert();
		}
	}
}

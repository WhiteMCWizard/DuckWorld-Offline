using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

[TimelineTrack("Actor Track", new TimelineTrackGenre[]
{
	TimelineTrackGenre.ActorTrack,
	TimelineTrackGenre.MultiActorTrack
}, new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ActorItemTrack : TimelineTrack, IActorTrack, IMultiActorTrack
{
	public Transform Actor
	{
		get
		{
			ActorTrackGroup actorTrackGroup = base.TrackGroup as ActorTrackGroup;
			if (actorTrackGroup == null)
			{
				Debug.LogError("No ActorTrackGroup found on parent.", this);
				return null;
			}
			return actorTrackGroup.Actor;
		}
	}

	public List<Transform> Actors
	{
		get
		{
			ActorTrackGroup actorTrackGroup = base.TrackGroup as ActorTrackGroup;
			if (actorTrackGroup != null)
			{
				List<Transform> list = new List<Transform>();
				list.Add(actorTrackGroup.Actor);
				return list;
			}
			MultiActorTrackGroup multiActorTrackGroup = base.TrackGroup as MultiActorTrackGroup;
			if (multiActorTrackGroup != null)
			{
				return multiActorTrackGroup.Actors;
			}
			return null;
		}
	}

	public CinemaActorEvent[] ActorEvents => GetComponentsInChildren<CinemaActorEvent>();

	public CinemaActorAction[] ActorActions => GetComponentsInChildren<CinemaActorAction>();

	public override void Initialize()
	{
		base.Initialize();
		CinemaActorEvent[] actorEvents = ActorEvents;
		foreach (CinemaActorEvent cinemaActorEvent in actorEvents)
		{
			foreach (Transform actor in Actors)
			{
				if (actor != null)
				{
					cinemaActorEvent.Initialize(actor.gameObject);
				}
			}
		}
	}

	public override void SetTime(float time)
	{
		float num = elapsedTime;
		base.SetTime(time);
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaActorEvent cinemaActorEvent = timelineItem as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				if (num < cinemaActorEvent.Firetime && time >= cinemaActorEvent.Firetime)
				{
					foreach (Transform actor in Actors)
					{
						if (actor != null)
						{
							cinemaActorEvent.Trigger(actor.gameObject);
						}
					}
				}
				else if (num >= cinemaActorEvent.Firetime && time < cinemaActorEvent.Firetime)
				{
					foreach (Transform actor2 in Actors)
					{
						if (actor2 != null)
						{
							cinemaActorEvent.Reverse(actor2.gameObject);
						}
					}
				}
			}
			CinemaActorAction cinemaActorAction = timelineItem as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			foreach (Transform actor3 in Actors)
			{
				if (actor3 != null)
				{
					cinemaActorAction.SetTime(actor3.gameObject, time - cinemaActorAction.Firetime, time - num);
				}
			}
		}
	}

	public override void UpdateTrack(float time, float deltaTime)
	{
		float num = elapsedTime;
		base.UpdateTrack(time, deltaTime);
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaActorEvent cinemaActorEvent = timelineItem as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				if (num < cinemaActorEvent.Firetime && elapsedTime >= cinemaActorEvent.Firetime)
				{
					foreach (Transform actor in Actors)
					{
						if (actor != null)
						{
							cinemaActorEvent.Trigger(actor.gameObject);
						}
					}
				}
				if (num >= cinemaActorEvent.Firetime && elapsedTime < cinemaActorEvent.Firetime)
				{
					foreach (Transform actor2 in Actors)
					{
						if (actor2 != null)
						{
							cinemaActorEvent.Reverse(actor2.gameObject);
						}
					}
				}
			}
			CinemaActorAction cinemaActorAction = timelineItem as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			if (num < cinemaActorAction.Firetime && elapsedTime >= cinemaActorAction.Firetime && elapsedTime < cinemaActorAction.EndTime)
			{
				foreach (Transform actor3 in Actors)
				{
					if (actor3 != null)
					{
						cinemaActorAction.Trigger(actor3.gameObject);
					}
				}
			}
			else if (num <= cinemaActorAction.EndTime && elapsedTime > cinemaActorAction.EndTime)
			{
				foreach (Transform actor4 in Actors)
				{
					if (actor4 != null)
					{
						cinemaActorAction.End(actor4.gameObject);
					}
				}
			}
			else if (num >= cinemaActorAction.Firetime && num < cinemaActorAction.EndTime && elapsedTime < cinemaActorAction.Firetime)
			{
				foreach (Transform actor5 in Actors)
				{
					if (actor5 != null)
					{
						cinemaActorAction.ReverseTrigger(actor5.gameObject);
					}
				}
			}
			else if (num > cinemaActorAction.EndTime && elapsedTime > cinemaActorAction.Firetime && elapsedTime <= cinemaActorAction.EndTime)
			{
				foreach (Transform actor6 in Actors)
				{
					if (actor6 != null)
					{
						cinemaActorAction.ReverseEnd(actor6.gameObject);
					}
				}
			}
			else
			{
				if (!(elapsedTime > cinemaActorAction.Firetime) || !(elapsedTime <= cinemaActorAction.EndTime))
				{
					continue;
				}
				foreach (Transform actor7 in Actors)
				{
					if (actor7 != null)
					{
						float time2 = time - cinemaActorAction.Firetime;
						cinemaActorAction.UpdateTime(actor7.gameObject, time2, deltaTime);
					}
				}
			}
		}
	}

	public override void Resume()
	{
		base.Resume();
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaActorAction cinemaActorAction = timelineItem as CinemaActorAction;
			if (!(cinemaActorAction != null) || !(elapsedTime > cinemaActorAction.Firetime) || !(elapsedTime < cinemaActorAction.Firetime + cinemaActorAction.Duration))
			{
				continue;
			}
			foreach (Transform actor in Actors)
			{
				if (actor != null)
				{
					cinemaActorAction.Resume(actor.gameObject);
				}
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		elapsedTime = 0f;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaActorEvent cinemaActorEvent = timelineItem as CinemaActorEvent;
			if (cinemaActorEvent != null)
			{
				foreach (Transform actor in Actors)
				{
					if (actor != null)
					{
						cinemaActorEvent.Stop(actor.gameObject);
					}
				}
			}
			CinemaActorAction cinemaActorAction = timelineItem as CinemaActorAction;
			if (!(cinemaActorAction != null))
			{
				continue;
			}
			foreach (Transform actor2 in Actors)
			{
				if (actor2 != null)
				{
					cinemaActorAction.Stop(actor2.gameObject);
				}
			}
		}
	}
}

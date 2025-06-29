using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

public abstract class TimelineTrack : MonoBehaviour, IOptimizable
{
	[SerializeField]
	private int ordinal = -1;

	[SerializeField]
	private bool canOptimize = true;

	public PlaybackMode PlaybackMode = PlaybackMode.RuntimeAndEdit;

	protected float elapsedTime = -1f;

	protected TimelineItem[] itemCache;

	protected List<Type> allowedItemTypes;

	private bool hasBeenOptimized;

	public Cutscene Cutscene => (!(TrackGroup == null)) ? TrackGroup.Cutscene : null;

	public TrackGroup TrackGroup
	{
		get
		{
			TrackGroup trackGroup = null;
			if (base.transform.parent != null)
			{
				trackGroup = base.transform.parent.GetComponentInParent<TrackGroup>();
				if (trackGroup == null)
				{
					Debug.LogError("No TrackGroup found on parent.", this);
				}
			}
			else
			{
				Debug.LogError("Track has no parent.", this);
			}
			return trackGroup;
		}
	}

	public int Ordinal
	{
		get
		{
			return ordinal;
		}
		set
		{
			ordinal = value;
		}
	}

	public bool CanOptimize
	{
		get
		{
			return canOptimize;
		}
		set
		{
			canOptimize = value;
		}
	}

	public virtual TimelineItem[] TimelineItems => GetComponentsInChildren<TimelineItem>();

	public virtual void Optimize()
	{
		if (canOptimize)
		{
			itemCache = GetTimelineItems();
			hasBeenOptimized = true;
		}
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			if (timelineItem is IOptimizable)
			{
				(timelineItem as IOptimizable).Optimize();
			}
		}
	}

	public virtual void Initialize()
	{
		elapsedTime = -1f;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			timelineItem.Initialize();
		}
	}

	public virtual void UpdateTrack(float runningTime, float deltaTime)
	{
		float num = elapsedTime;
		elapsedTime = runningTime;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaGlobalEvent cinemaGlobalEvent = timelineItem as CinemaGlobalEvent;
			if (!(cinemaGlobalEvent == null))
			{
				if (num < cinemaGlobalEvent.Firetime && elapsedTime >= cinemaGlobalEvent.Firetime)
				{
					cinemaGlobalEvent.Trigger();
				}
				else if (num >= cinemaGlobalEvent.Firetime && elapsedTime < cinemaGlobalEvent.Firetime)
				{
					cinemaGlobalEvent.Reverse();
				}
			}
		}
		TimelineItem[] timelineItems2 = GetTimelineItems();
		foreach (TimelineItem timelineItem2 in timelineItems2)
		{
			CinemaGlobalAction cinemaGlobalAction = timelineItem2 as CinemaGlobalAction;
			if (!(cinemaGlobalAction == null))
			{
				if (num < cinemaGlobalAction.Firetime && elapsedTime >= cinemaGlobalAction.Firetime && elapsedTime < cinemaGlobalAction.EndTime)
				{
					cinemaGlobalAction.Trigger();
				}
				else if (num < cinemaGlobalAction.EndTime && elapsedTime >= cinemaGlobalAction.EndTime)
				{
					cinemaGlobalAction.End();
				}
				else if (num > cinemaGlobalAction.Firetime && num <= cinemaGlobalAction.EndTime && elapsedTime < cinemaGlobalAction.Firetime)
				{
					cinemaGlobalAction.ReverseTrigger();
				}
				else if (num > cinemaGlobalAction.EndTime && elapsedTime > cinemaGlobalAction.Firetime && elapsedTime <= cinemaGlobalAction.EndTime)
				{
					cinemaGlobalAction.ReverseEnd();
				}
				else if (elapsedTime > cinemaGlobalAction.Firetime && elapsedTime < cinemaGlobalAction.EndTime)
				{
					float time = runningTime - cinemaGlobalAction.Firetime;
					cinemaGlobalAction.UpdateTime(time, deltaTime);
				}
			}
		}
	}

	public virtual void Pause()
	{
	}

	public virtual void Resume()
	{
	}

	public virtual void SetTime(float time)
	{
		float num = elapsedTime;
		elapsedTime = time;
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			CinemaGlobalEvent cinemaGlobalEvent = timelineItem as CinemaGlobalEvent;
			if (cinemaGlobalEvent != null)
			{
				if (num < cinemaGlobalEvent.Firetime && elapsedTime >= cinemaGlobalEvent.Firetime)
				{
					cinemaGlobalEvent.Trigger();
				}
				else if (num >= cinemaGlobalEvent.Firetime && elapsedTime < cinemaGlobalEvent.Firetime)
				{
					cinemaGlobalEvent.Reverse();
				}
			}
			CinemaGlobalAction cinemaGlobalAction = timelineItem as CinemaGlobalAction;
			if (cinemaGlobalAction != null)
			{
				cinemaGlobalAction.SetTime(time - cinemaGlobalAction.Firetime, time - num);
			}
		}
	}

	public virtual List<float> GetMilestones(float from, float to)
	{
		bool flag = from > to;
		List<float> list = new List<float>();
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			if (((!flag && from < timelineItem.Firetime && to >= timelineItem.Firetime) || (flag && from > timelineItem.Firetime && to <= timelineItem.Firetime)) && !list.Contains(timelineItem.Firetime))
			{
				list.Add(timelineItem.Firetime);
			}
			if (timelineItem is TimelineAction)
			{
				float endTime = (timelineItem as TimelineAction).EndTime;
				if (((!flag && from < endTime && to >= endTime) || (flag && from > endTime && to <= endTime)) && !list.Contains(endTime))
				{
					list.Add(endTime);
				}
			}
		}
		list.Sort();
		return list;
	}

	public virtual void Stop()
	{
		TimelineItem[] timelineItems = GetTimelineItems();
		foreach (TimelineItem timelineItem in timelineItems)
		{
			timelineItem.Stop();
		}
	}

	public List<Type> GetAllowedCutsceneItems()
	{
		if (allowedItemTypes == null)
		{
			allowedItemTypes = DirectorRuntimeHelper.GetAllowedItemTypes(this);
		}
		return allowedItemTypes;
	}

	public TimelineItem[] GetTimelineItems()
	{
		if (hasBeenOptimized)
		{
			return itemCache;
		}
		List<TimelineItem> list = new List<TimelineItem>();
		foreach (Type allowedCutsceneItem in GetAllowedCutsceneItems())
		{
			Component[] componentsInChildren = GetComponentsInChildren(allowedCutsceneItem);
			Component[] array = componentsInChildren;
			foreach (Component component in array)
			{
				list.Add((TimelineItem)component);
			}
		}
		return list.ToArray();
	}
}

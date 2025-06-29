using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

[TrackGroup("Track Group", new TimelineTrackGenre[] { TimelineTrackGenre.GlobalTrack })]
public abstract class TrackGroup : MonoBehaviour, IOptimizable
{
	[SerializeField]
	private int ordinal = -1;

	[SerializeField]
	private bool canOptimize = true;

	protected TimelineTrack[] trackCache;

	protected List<Type> allowedTrackTypes;

	private bool hasBeenOptimized;

	public Cutscene Cutscene
	{
		get
		{
			Cutscene cutscene = null;
			if (base.transform.parent != null)
			{
				cutscene = base.transform.parent.GetComponentInParent<Cutscene>();
				if (cutscene == null)
				{
					Debug.LogError("No Cutscene found on parent!", this);
				}
			}
			else
			{
				Debug.LogError("TrackGroup has no parent!", this);
			}
			return cutscene;
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

	public virtual void Optimize()
	{
		if (canOptimize)
		{
			trackCache = GetTracks();
			hasBeenOptimized = true;
		}
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.Optimize();
		}
	}

	public virtual void Initialize()
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.Initialize();
		}
	}

	public virtual void UpdateTrackGroup(float time, float deltaTime)
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.UpdateTrack(time, deltaTime);
		}
	}

	public virtual void Pause()
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.Pause();
		}
	}

	public virtual void Stop()
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.Stop();
		}
	}

	public virtual void Resume()
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.Resume();
		}
	}

	public virtual void SetRunningTime(float time)
	{
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			timelineTrack.SetTime(time);
		}
	}

	public virtual List<float> GetMilestones(float from, float to)
	{
		List<float> list = new List<float>();
		TimelineTrack[] tracks = GetTracks();
		foreach (TimelineTrack timelineTrack in tracks)
		{
			List<float> milestones = timelineTrack.GetMilestones(from, to);
			foreach (float item2 in milestones)
			{
				float item = item2;
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
		}
		list.Sort();
		return list;
	}

	public virtual TimelineTrack[] GetTracks()
	{
		if (hasBeenOptimized)
		{
			return trackCache;
		}
		List<TimelineTrack> list = new List<TimelineTrack>();
		foreach (Type allowedTrackType in GetAllowedTrackTypes())
		{
			Component[] componentsInChildren = GetComponentsInChildren(allowedTrackType);
			Component[] array = componentsInChildren;
			foreach (Component component in array)
			{
				list.Add((TimelineTrack)component);
			}
		}
		list.Sort((TimelineTrack track1, TimelineTrack track2) => track1.Ordinal - track2.Ordinal);
		return list.ToArray();
	}

	public List<Type> GetAllowedTrackTypes()
	{
		if (allowedTrackTypes == null)
		{
			allowedTrackTypes = DirectorRuntimeHelper.GetAllowedTrackTypes(this);
		}
		return allowedTrackTypes;
	}
}

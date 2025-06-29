using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

[ExecuteInEditMode]
public abstract class CinemaActorEvent : TimelineItem
{
	public ActorTrackGroup ActorTrackGroup => base.TimelineTrack.TrackGroup as ActorTrackGroup;

	public abstract void Trigger(GameObject Actor);

	public virtual void Reverse(GameObject Actor)
	{
	}

	public virtual void SetTimeTo(float deltaTime)
	{
	}

	public virtual void Pause()
	{
	}

	public virtual void Resume()
	{
	}

	public virtual void Initialize(GameObject Actor)
	{
	}

	public virtual void Stop(GameObject Actor)
	{
	}

	public virtual List<Transform> GetActors()
	{
		if (base.TimelineTrack is IMultiActorTrack multiActorTrack)
		{
			return multiActorTrack.Actors;
		}
		return null;
	}
}

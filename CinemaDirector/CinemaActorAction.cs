using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector;

[ExecuteInEditMode]
public abstract class CinemaActorAction : TimelineAction
{
	public abstract void Trigger(GameObject Actor);

	public virtual void UpdateTime(GameObject Actor, float time, float deltaTime)
	{
	}

	public abstract void End(GameObject Actor);

	public virtual void Stop(GameObject Actor)
	{
	}

	public virtual void SetTime(GameObject Actor, float time, float deltaTime)
	{
	}

	public virtual void ReverseTrigger(GameObject Actor)
	{
	}

	public virtual void ReverseEnd(GameObject Actor)
	{
	}

	public virtual void Pause(GameObject Actor)
	{
	}

	public virtual void Resume(GameObject Actor)
	{
	}

	public int CompareTo(object other)
	{
		CinemaGlobalAction cinemaGlobalAction = (CinemaGlobalAction)other;
		return (int)(cinemaGlobalAction.Firetime - base.Firetime);
	}

	public virtual List<Transform> GetActors()
	{
		if (base.TimelineTrack is IMultiActorTrack multiActorTrack)
		{
			return multiActorTrack.Actors;
		}
		return null;
	}

	[Obsolete("Use SetTime with Actor")]
	public virtual void SetTime(float time, float deltaTime)
	{
	}

	[Obsolete("Use ReverseTrigger with Actor")]
	public virtual void ReverseTrigger()
	{
	}

	[Obsolete("Use ReverseEnd with Actor")]
	public virtual void ReverseEnd()
	{
	}
}

using System;
using UnityEngine;

namespace CinemaDirector;

[ExecuteInEditMode]
public abstract class CinemaGlobalAction : TimelineAction, IComparable
{
	public abstract void Trigger();

	public virtual void UpdateTime(float time, float deltaTime)
	{
	}

	public abstract void End();

	public virtual void SetTime(float time, float deltaTime)
	{
	}

	public virtual void Pause()
	{
	}

	public virtual void Resume()
	{
	}

	public virtual void ReverseTrigger()
	{
	}

	public virtual void ReverseEnd()
	{
	}

	public int CompareTo(object other)
	{
		CinemaGlobalAction cinemaGlobalAction = (CinemaGlobalAction)other;
		return (int)(cinemaGlobalAction.Firetime - base.Firetime);
	}
}

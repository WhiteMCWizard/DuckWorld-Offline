using UnityEngine;

namespace CinemaDirector;

public abstract class TimelineAction : TimelineItem
{
	[SerializeField]
	protected float duration;

	public float Duration
	{
		get
		{
			return duration;
		}
		set
		{
			duration = value;
		}
	}

	public float EndTime => firetime + duration;

	public override void SetDefaults()
	{
		duration = 5f;
	}
}

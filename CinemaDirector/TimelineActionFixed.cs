using UnityEngine;

namespace CinemaDirector;

public abstract class TimelineActionFixed : TimelineAction
{
	[SerializeField]
	private float inTime;

	[SerializeField]
	private float outTime = 1f;

	[SerializeField]
	private float itemLength;

	public float InTime
	{
		get
		{
			return inTime;
		}
		set
		{
			inTime = value;
			base.Duration = outTime - inTime;
		}
	}

	public float OutTime
	{
		get
		{
			return outTime;
		}
		set
		{
			outTime = value;
			base.Duration = outTime - inTime;
		}
	}

	public float ItemLength
	{
		get
		{
			return itemLength;
		}
		set
		{
			itemLength = value;
		}
	}
}

using UnityEngine;

namespace SLAM;

public class Stopwatch
{
	public float StartTime { get; private set; }

	public float EndTime { get; private set; }

	public float Duration { get; private set; }

	public float ElapsedTime => Time.time - StartTime;

	public float Progress => (!(Duration > 0f)) ? 1f : (ElapsedTime / Duration);

	public float RemainingTime => Duration - ElapsedTime;

	public bool Expired => Progress >= 1f;

	public Stopwatch(float duration)
	{
		StartTime = Time.time;
		EndTime = Time.time + duration;
		Duration = duration;
	}

	public Stopwatch(float duration, float elapsedTime)
		: this(duration)
	{
		StartTime -= elapsedTime;
	}

	public static implicit operator bool(Stopwatch sw)
	{
		return !sw.Expired;
	}
}

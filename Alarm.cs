using System;
using UnityEngine;

public class Alarm : MonoBehaviour
{
	[SerializeField]
	private float currentTime;

	private float timerDuration;

	private bool autoDestroy = true;

	private Action callback;

	private bool hasStarted;

	private bool countUp;

	private static int timerCount;

	public float CurrentTime => currentTime;

	public float TimerDuration => timerDuration;

	public float TimeLeft
	{
		get
		{
			return currentTime;
		}
		set
		{
			float num = value - currentTime;
			timerDuration += num;
			currentTime = value;
		}
	}

	public bool HasStarted => hasStarted;

	public float Progress => currentTime / timerDuration;

	public bool Expired => currentTime <= 0f;

	private void Update()
	{
		if (hasStarted)
		{
			if (countUp)
			{
				currentTime += Time.deltaTime;
			}
			else
			{
				currentTime -= Time.deltaTime;
			}
			if ((countUp && currentTime > timerDuration) || (!countUp && currentTime < 0f))
			{
				timerFinished();
			}
		}
	}

	public void StartCountdown(float countDownFrom, Action finishedCallback, bool destroyWhenFinished = true)
	{
		currentTime = (timerDuration = countDownFrom);
		callback = finishedCallback;
		autoDestroy = destroyWhenFinished;
		hasStarted = true;
	}

	public void StartCountUp()
	{
		timerDuration = float.MaxValue;
		currentTime = 0f;
		hasStarted = (countUp = true);
	}

	public void StartCountUp(float countUpTime, Action finishedCallback, bool destroyWhenFinished = true)
	{
		StartCountUp();
		timerDuration = countUpTime;
		callback = finishedCallback;
		autoDestroy = destroyWhenFinished;
	}

	public void Restart()
	{
		if (countUp)
		{
			currentTime = 0f;
		}
		else
		{
			currentTime = timerDuration;
		}
		hasStarted = true;
	}

	public void Reset()
	{
		if (countUp)
		{
			currentTime = 0f;
		}
		else
		{
			currentTime = timerDuration;
		}
		hasStarted = false;
	}

	public void Resume()
	{
		hasStarted = true;
	}

	public void Pause()
	{
		hasStarted = false;
	}

	public void Destroy()
	{
		Pause();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void timerFinished()
	{
		hasStarted = false;
		currentTime = 0f;
		if (callback != null)
		{
			callback();
		}
		if (autoDestroy)
		{
			Destroy();
		}
	}

	public static Alarm Create()
	{
		timerCount++;
		GameObject gameObject = new GameObject("Timer " + timerCount);
		return gameObject.AddComponent<Alarm>();
	}

	public static Alarm Create(Transform parent)
	{
		Alarm alarm = Create();
		alarm.transform.parent = parent;
		return alarm;
	}
}

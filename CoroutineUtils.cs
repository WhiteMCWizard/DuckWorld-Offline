using System;
using System.Collections;
using SLAM;
using UnityEngine;

public static class CoroutineUtils
{
	public static Coroutine WaitForUnscaledSeconds(float time)
	{
		return StaticCoroutine.Start(doWaitForUnscaledSeconds(time));
	}

	public static Coroutine WaitForGameEvent<T>(Action<T> callback = null) where T : class
	{
		return StaticCoroutine.Start(doWaitForGameEvent(callback));
	}

	public static Coroutine WaitForObjectDestroyed(GameObject obj, Action callback)
	{
		return StaticCoroutine.Start(doWaitForObjectDestroyed(obj, callback));
	}

	private static IEnumerator doWaitForGameEvent<T>(Action<T> callback) where T : class
	{
		bool canContinue = false;
		bool flag;
		callback = (Action<T>)Delegate.Combine(callback, (Action<T>)delegate
		{
			flag = true;
		});
		GameEvents.Subscribe(callback);
		while (!canContinue)
		{
			yield return null;
		}
		GameEvents.Unsubscribe(callback);
	}

	private static IEnumerator doWaitForUnscaledSeconds(float time)
	{
		float t = 0f;
		while (t < time)
		{
			t += Time.unscaledDeltaTime;
			yield return null;
		}
	}

	private static IEnumerator doWaitForObjectDestroyed(GameObject obj, Action callback)
	{
		while (obj != null)
		{
			yield return null;
		}
		callback?.Invoke();
	}
}

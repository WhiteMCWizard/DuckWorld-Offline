using System;
using System.Collections;
using UnityEngine;

namespace SLAM;

public static class GameEvents
{
	private static EventDispatcher dispatcher = new EventDispatcher();

	public static void Subscribe<T>(Action<T> callback) where T : class
	{
		dispatcher.Subscribe(callback);
	}

	public static void Unsubscribe<T>(Action<T> callback) where T : class
	{
		dispatcher.Unsubscribe(callback);
	}

	public static void Invoke<T>(T evt) where T : class
	{
		dispatcher.Invoke(evt);
	}

	public static void InvokeAtEndOfFrame<T>(T evt) where T : class
	{
		StaticCoroutine.Start(delayedInvoke(evt));
	}

	private static IEnumerator delayedInvoke<T>(T evt) where T : class
	{
		yield return new WaitForEndOfFrame();
		Invoke(evt);
	}
}

using System;
using System.Collections.Generic;

namespace SLAM;

public class EventDispatcher
{
	private Dictionary<Type, Action<object>> events = new Dictionary<Type, Action<object>>();

	private Dictionary<object, Action<object>> lookup = new Dictionary<object, Action<object>>();

	public void Subscribe<T>(Action<T> callback) where T : class
	{
		Action<object> action = delegate(object o)
		{
			callback((T)o);
		};
		if (events.ContainsKey(typeof(T)))
		{
			events[typeof(T)] = (Action<object>)Delegate.Combine(events[typeof(T)], action);
		}
		else
		{
			events.Add(typeof(T), action);
		}
		lookup.Add(callback, action);
	}

	public void Unsubscribe<T>(Action<T> callback) where T : class
	{
		Dictionary<Type, Action<object>> dictionary2;
		Dictionary<Type, Action<object>> dictionary = (dictionary2 = events);
		Type typeFromHandle;
		Type key = (typeFromHandle = typeof(T));
		Action<object> source = dictionary2[typeFromHandle];
		dictionary[key] = (Action<object>)Delegate.Remove(source, lookup[callback]);
		lookup.Remove(callback);
	}

	public void Invoke<T>(T evt) where T : class
	{
		if (events.TryGetValue(typeof(T), out var value))
		{
			value?.Invoke(evt);
		}
	}
}

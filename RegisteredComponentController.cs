using System;
using System.Collections.Generic;
using UnityEngine;

public static class RegisteredComponentController
{
	public class InstanceContainer : HashSet_Flash<IRegisteredComponent>
	{
	}

	private static Dictionary<Type, InstanceContainer> _instanceContainers = new Dictionary<Type, InstanceContainer>();

	public static T[] GetAllOfType<T>() where T : IRegisteredComponent
	{
		if (!_instanceContainers.TryGetValue(typeof(T), out var value))
		{
			return new T[0];
		}
		T[] array = new T[value.Count];
		int num = 0;
		foreach (IRegisteredComponent item in value)
		{
			array[num++] = (T)item;
		}
		return array;
	}

	public static object[] GetAllOfType(Type type)
	{
		if (!_instanceContainers.TryGetValue(type, out var value))
		{
			return new object[0];
		}
		object[] array = new object[value.Count];
		int num = 0;
		foreach (IRegisteredComponent item in value)
		{
			array[num++] = item;
		}
		return array;
	}

	public static int InstanceCountOfType<T>() where T : IRegisteredComponent
	{
		if (!_instanceContainers.TryGetValue(typeof(T), out var value))
		{
			return 0;
		}
		return value.Count;
	}

	private static InstanceContainer _GetInstanceContainer(Type type)
	{
		if (_instanceContainers.TryGetValue(type, out var value))
		{
			return value;
		}
		value = new InstanceContainer();
		_instanceContainers.Add(type, value);
		return value;
	}

	private static void _RegisterType(IRegisteredComponent component, Type type)
	{
		InstanceContainer instanceContainer = _GetInstanceContainer(type);
		if (!instanceContainer.Add(component))
		{
			Debug.LogError("RegisteredComponentController error: Tried to register same instance twice");
		}
	}

	internal static void _Register(IRegisteredComponent component)
	{
		Type type = component.GetType();
		do
		{
			_RegisterType(component, type);
			type = type.BaseType;
		}
		while (type != component.GetRegisteredComponentBaseClassType());
	}

	internal static void _UnregisterType(IRegisteredComponent component, Type type)
	{
		InstanceContainer instanceContainer = _GetInstanceContainer(type);
		if (!instanceContainer.Remove(component))
		{
			Debug.LogError("RegisteredComponentController error: Tried to unregister unknown instance");
		}
	}

	internal static void _Unregister(IRegisteredComponent component)
	{
		Type type = component.GetType();
		do
		{
			_UnregisterType(component, type);
			type = type.BaseType;
		}
		while (type != component.GetRegisteredComponentBaseClassType());
	}
}

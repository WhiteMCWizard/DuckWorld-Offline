using System;
using System.Reflection;
using UnityEngine;

[AddComponentMenu("ClockStone/PoolableObject")]
public class PoolableObject : MonoBehaviour
{
	public int maxPoolSize = 10;

	public int preloadCount;

	public bool doNotDestroyOnLoad;

	public bool sendAwakeStartOnDestroyMessage = true;

	public bool sendPoolableActivateDeactivateMessages;

	internal bool _isAvailableForPooling;

	internal bool _createdWithPoolController;

	internal bool _destroyMessageFromPoolController;

	internal bool _wasPreloaded;

	internal bool _wasStartCalledByUnity;

	internal ObjectPoolController.ObjectPool _myPool;

	internal int _serialNumber;

	internal int _usageCount;

	protected void Start()
	{
		_wasStartCalledByUnity = true;
	}

	private static void _InvokeMethodByName(MonoBehaviour behaviour, string methodName)
	{
		Type type = behaviour.GetType();
		type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(behaviour, null);
	}

	private static void _BroadcastMessageToGameObject(GameObject go, string message)
	{
		Component[] components = go.GetComponents(typeof(MonoBehaviour));
		Component[] array = components;
		foreach (Component component in array)
		{
			_InvokeMethodByName((MonoBehaviour)component, message);
		}
		if (go.transform.childCount > 0)
		{
			_BroadcastMessageToAllChildren(go, message);
		}
	}

	private static void _BroadcastMessageToAllChildren(GameObject go, string message)
	{
		Transform[] array = new Transform[go.transform.childCount];
		for (int i = 0; i < go.transform.childCount; i++)
		{
			array[i] = go.transform.GetChild(i);
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].GetComponent<PoolableObject>() == null)
			{
				_BroadcastMessageToGameObject(array[j].gameObject, message);
			}
		}
	}

	protected void OnDestroy()
	{
		if (!_destroyMessageFromPoolController && _myPool != null)
		{
			_myPool.Remove(this);
		}
		if (!_destroyMessageFromPoolController)
		{
			_BroadcastMessageToGameObject(base.gameObject, "OnPoolableInstanceDestroy");
		}
		_destroyMessageFromPoolController = false;
	}

	public int GetSerialNumber()
	{
		return _serialNumber;
	}

	public int GetUsageCount()
	{
		return _usageCount;
	}

	public int DeactivateAllPoolableObjectsOfMyKind()
	{
		if (_myPool != null)
		{
			return _myPool._SetAllAvailable();
		}
		return 0;
	}

	public bool IsDeactivated()
	{
		return _isAvailableForPooling;
	}

	public PoolableObject[] GetAllPoolableObjectsOfMyKind(bool includeInactiveObjects)
	{
		if (_myPool != null)
		{
			return _myPool._GetAllObjects(includeInactiveObjects);
		}
		return null;
	}
}

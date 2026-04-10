using System;
using UnityEngine;

public class UnitySingleton<T> where T : MonoBehaviour
{
	private static T _instance;

	internal static Type _myType = typeof(T);

	internal static GameObject _autoCreatePrefab;

	private static int _GlobalInstanceCount = 0;

	private static bool _awakeSingletonCalled = false;

	private UnitySingleton()
	{
	}

	public static T GetSingleton(bool throwErrorIfNotFound, bool autoCreate)
	{
		if (!_instance)
		{
			UnityEngine.Object obj = null;
			UnityEngine.Object[] array = UnityEngine.Object.FindObjectsOfType(_myType);
			UnityEngine.Object[] array2 = array;
			foreach (UnityEngine.Object obj2 in array2)
			{
				ISingletonMonoBehaviour singletonMonoBehaviour = (ISingletonMonoBehaviour)obj2;
				if (singletonMonoBehaviour.isSingletonObject)
				{
					obj = (UnityEngine.Object)singletonMonoBehaviour;
					break;
				}
			}
			if (!obj)
			{
				if (!autoCreate || !(_autoCreatePrefab != null))
				{
					if (throwErrorIfNotFound)
					{
						Debug.LogError("No singleton component " + _myType.Name + " found in the scene.");
					}
					return (T)null;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(_autoCreatePrefab);
				gameObject.name = _autoCreatePrefab.name;
				UnityEngine.Object obj3 = UnityEngine.Object.FindObjectOfType(_myType);
				if (!obj3)
				{
					Debug.LogError("Auto created object does not have component " + _myType.Name);
					return (T)null;
				}
			}
			else
			{
				_AwakeSingleton(obj as T);
			}
			_instance = (T)obj;
		}
		return _instance;
	}

	internal static void _Awake(T instance)
	{
		_GlobalInstanceCount++;
		if (_GlobalInstanceCount > 1)
		{
			Debug.LogError("More than one instance of SingletonMonoBehaviour " + typeof(T).Name);
		}
		else
		{
			_instance = instance;
		}
		_AwakeSingleton((T)instance);
	}

	internal static void _Destroy()
	{
		if (_GlobalInstanceCount > 0)
		{
			_GlobalInstanceCount--;
			if (_GlobalInstanceCount == 0)
			{
				_awakeSingletonCalled = false;
				_instance = (T)null;
			}
		}
	}

	private static void _AwakeSingleton(T instance)
	{
		if (!_awakeSingletonCalled)
		{
			_awakeSingletonCalled = true;
			instance.SendMessage("AwakeSingleton", SendMessageOptions.DontRequireReceiver);
		}
	}
}

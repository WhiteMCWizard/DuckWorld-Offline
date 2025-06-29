using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour, ISingletonMonoBehaviour where T : MonoBehaviour
{
	public static T Instance => UnitySingleton<T>.GetSingleton(throwErrorIfNotFound: true, autoCreate: true);

	public virtual bool isSingletonObject => true;

	public static T DoesInstanceExist()
	{
		return UnitySingleton<T>.GetSingleton(throwErrorIfNotFound: false, autoCreate: false);
	}

	public static void ActivateSingletonInstance()
	{
		UnitySingleton<T>.GetSingleton(throwErrorIfNotFound: true, autoCreate: true);
	}

	public static void SetSingletonAutoCreate(GameObject autoCreatePrefab)
	{
		UnitySingleton<T>._autoCreatePrefab = autoCreatePrefab;
	}

	public static void SetSingletonType(Type type)
	{
		UnitySingleton<T>._myType = type;
	}

	protected virtual void Awake()
	{
		if (isSingletonObject)
		{
			UnitySingleton<T>._Awake(this as T);
		}
	}

	protected virtual void OnDestroy()
	{
		if (isSingletonObject)
		{
			UnitySingleton<T>._Destroy();
		}
	}
}
public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				if (!UnityEngine.Object.FindObjectOfType(typeof(T)))
				{
					Debug.LogError("Make sure there is one instance of " + typeof(T).Name + " in the current scene.");
				}
				else
				{
					if (Application.isEditor)
					{
						Debug.LogError("_instance is null. Did you implement Awake() without override?");
						return UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
					}
					Debug.LogError("Please do not call " + typeof(T).Name + " in Awake()");
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		_instance = (T)this;
	}

	protected virtual void OnDestroy()
	{
		_instance = (T)null;
	}
}

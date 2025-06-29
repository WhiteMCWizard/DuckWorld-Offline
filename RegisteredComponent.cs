using System;
using UnityEngine;

public abstract class RegisteredComponent : MonoBehaviour, IRegisteredComponent
{
	private bool isRegistered;

	private bool isUnregistered;

	protected virtual void Awake()
	{
		if (!isRegistered)
		{
			RegisteredComponentController._Register(this);
			isRegistered = true;
			isUnregistered = false;
		}
		else
		{
			Debug.LogWarning("RegisteredComponent: Awake() / OnDestroy() not correctly called. Object: " + base.name);
		}
	}

	protected virtual void OnDestroy()
	{
		if (isRegistered && !isUnregistered)
		{
			RegisteredComponentController._Unregister(this);
			isRegistered = false;
			isUnregistered = true;
		}
		else if (isRegistered || !isUnregistered)
		{
			Debug.LogWarning("RegisteredComponent: Awake() / OnDestroy() not correctly called. Object: " + base.name + " isRegistered:" + isRegistered + " isUnregistered:" + isUnregistered);
		}
	}

	public Type GetRegisteredComponentBaseClassType()
	{
		return typeof(RegisteredComponent);
	}
}

using UnityEngine;

public class PoolableReference<T> where T : Component
{
	private PoolableObject _pooledObj;

	private int _initialUsageCount;

	private T _objComponent;

	public PoolableReference()
	{
		Reset();
	}

	public PoolableReference(T componentOfPoolableObject)
	{
		Set(componentOfPoolableObject, allowNonePoolable: false);
	}

	public PoolableReference(PoolableReference<T> poolableReference)
	{
		_objComponent = poolableReference._objComponent;
		_pooledObj = poolableReference._pooledObj;
		_initialUsageCount = poolableReference._initialUsageCount;
	}

	public void Reset()
	{
		_pooledObj = null;
		_objComponent = (T)null;
		_initialUsageCount = 0;
	}

	public T Get()
	{
		if (!_objComponent)
		{
			return (T)null;
		}
		if ((bool)_pooledObj && (_pooledObj._usageCount != _initialUsageCount || _pooledObj._isAvailableForPooling))
		{
			_objComponent = (T)null;
			_pooledObj = null;
			return (T)null;
		}
		return _objComponent;
	}

	public void Set(T componentOfPoolableObject)
	{
		Set(componentOfPoolableObject, allowNonePoolable: false);
	}

	public void Set(T componentOfPoolableObject, bool allowNonePoolable)
	{
		if (!componentOfPoolableObject)
		{
			Reset();
			return;
		}
		_objComponent = componentOfPoolableObject;
		_pooledObj = _objComponent.GetComponent<PoolableObject>();
		if (!_pooledObj)
		{
			if (allowNonePoolable)
			{
				_initialUsageCount = 0;
			}
			else
			{
				Debug.LogError("Object for PoolableReference must be poolable");
			}
		}
		else
		{
			_initialUsageCount = _pooledObj._usageCount;
		}
	}
}

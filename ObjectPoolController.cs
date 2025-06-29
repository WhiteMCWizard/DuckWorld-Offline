using System.Collections.Generic;
using UnityEngine;

public static class ObjectPoolController
{
	internal class ObjectPool
	{
		private HashSet_Flash<PoolableObject> _pool;

		private PoolableObject _prefabPoolObj;

		private Transform _poolParentDummy;

		internal Transform poolParentDummy
		{
			get
			{
				_ValidatePoolParentDummy();
				return _poolParentDummy;
			}
		}

		public ObjectPool(GameObject prefab)
		{
			_prefabPoolObj = prefab.GetComponent<PoolableObject>();
		}

		private void _ValidatePoolParentDummy()
		{
			if (!_poolParentDummy)
			{
				GameObject gameObject = new GameObject("POOL:" + _prefabPoolObj.name);
				_poolParentDummy = gameObject.transform;
				_SetActive(gameObject, active: false);
				if (_prefabPoolObj.doNotDestroyOnLoad)
				{
					Object.DontDestroyOnLoad(gameObject);
				}
			}
		}

		private void _ValidatePooledObjectDataContainer()
		{
			if (_pool == null)
			{
				_pool = new HashSet_Flash<PoolableObject>();
				_ValidatePoolParentDummy();
			}
		}

		internal void Remove(PoolableObject poolObj)
		{
			_pool.Remove(poolObj);
		}

		internal int GetObjectCount()
		{
			if (_pool == null)
			{
				return 0;
			}
			return _pool.Count;
		}

		internal GameObject GetPooledInstance(Vector3? position, Quaternion? rotation)
		{
			_ValidatePooledObjectDataContainer();
			Transform transform = _prefabPoolObj.transform;
			foreach (PoolableObject item in _pool)
			{
				if (item != null && item._isAvailableForPooling)
				{
					Transform transform2 = item.transform;
					transform2.position = ((!position.HasValue) ? transform.position : position.Value);
					transform2.rotation = ((!rotation.HasValue) ? transform.rotation : rotation.Value);
					transform2.localScale = transform.localScale;
					item._usageCount++;
					_SetAvailable(item, b: false);
					return item.gameObject;
				}
			}
			if (_pool.Count < _prefabPoolObj.maxPoolSize)
			{
				return _NewPooledInstance(position, rotation).gameObject;
			}
			return null;
		}

		internal PoolableObject PreloadInstance()
		{
			_ValidatePooledObjectDataContainer();
			PoolableObject poolableObject = _NewPooledInstance(null, null);
			poolableObject._wasPreloaded = true;
			_SetAvailable(poolableObject, b: true);
			return poolableObject;
		}

		private PoolableObject _NewPooledInstance(Vector3? position, Quaternion? rotation)
		{
			_isDuringInstantiate = true;
			GameObject gameObject = ((!position.HasValue || !rotation.HasValue) ? Object.Instantiate(_prefabPoolObj.gameObject) : ((GameObject)Object.Instantiate(_prefabPoolObj.gameObject, position.Value, rotation.Value)));
			_isDuringInstantiate = false;
			PoolableObject component = gameObject.GetComponent<PoolableObject>();
			component._createdWithPoolController = true;
			component._myPool = this;
			component._isAvailableForPooling = false;
			component._serialNumber = ++_globalSerialNumber;
			component._usageCount++;
			if (component.doNotDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			_pool.Add(component);
			gameObject.BroadcastMessage("OnPoolableInstanceAwake", SendMessageOptions.DontRequireReceiver);
			return component;
		}

		internal int _SetAllAvailable()
		{
			int num = 0;
			foreach (PoolableObject item in _pool)
			{
				if (item != null && !item._isAvailableForPooling)
				{
					_SetAvailable(item, b: true);
					num++;
				}
			}
			return num;
		}

		internal PoolableObject[] _GetAllObjects(bool includeInactiveObjects)
		{
			List<PoolableObject> list = new List<PoolableObject>();
			foreach (PoolableObject item in _pool)
			{
				if (item != null && (includeInactiveObjects || !item._isAvailableForPooling))
				{
					list.Add(item);
				}
			}
			return list.ToArray();
		}

		internal void _SetAvailable(PoolableObject poolObj, bool b)
		{
			poolObj._isAvailableForPooling = b;
			Transform transform = poolObj.transform;
			if (b)
			{
				if (poolObj.sendAwakeStartOnDestroyMessage)
				{
					poolObj._destroyMessageFromPoolController = true;
				}
				transform.parent = null;
				_RecursivelySetInactiveAndSendMessages(poolObj.gameObject, poolObj, recursiveCall: false);
				transform.parent = poolObj._myPool.poolParentDummy;
			}
			else
			{
				transform.parent = null;
				_SetActiveAndSendMessages(poolObj.gameObject, poolObj);
			}
		}

		private void _SetActive(GameObject obj, bool active)
		{
			obj.SetActive(active);
		}

		private bool _GetActive(GameObject obj)
		{
			return obj.activeInHierarchy;
		}

		private void _SetActiveAndSendMessages(GameObject obj, PoolableObject parentPoolObj)
		{
			_SetActive(obj, active: true);
			if (parentPoolObj.sendAwakeStartOnDestroyMessage)
			{
				obj.BroadcastMessage("Awake", null, SendMessageOptions.DontRequireReceiver);
				if (_GetActive(obj) && parentPoolObj._wasStartCalledByUnity)
				{
					obj.BroadcastMessage("Start", null, SendMessageOptions.DontRequireReceiver);
				}
			}
			if (parentPoolObj.sendPoolableActivateDeactivateMessages)
			{
				obj.BroadcastMessage("OnPoolableObjectActivated", null, SendMessageOptions.DontRequireReceiver);
			}
		}

		private void _RecursivelySetInactiveAndSendMessages(GameObject obj, PoolableObject parentPoolObj, bool recursiveCall)
		{
			Transform transform = obj.transform;
			Transform[] array = new Transform[transform.childCount];
			for (int i = 0; i < transform.childCount; i++)
			{
				array[i] = transform.GetChild(i);
			}
			foreach (Transform transform2 in array)
			{
				PoolableObject component = transform2.gameObject.GetComponent<PoolableObject>();
				if ((bool)component && component._myPool != null)
				{
					_SetAvailable(component, b: true);
				}
				else
				{
					_RecursivelySetInactiveAndSendMessages(transform2.gameObject, parentPoolObj, recursiveCall: true);
				}
			}
			if (parentPoolObj.sendAwakeStartOnDestroyMessage)
			{
				obj.SendMessage("OnDestroy", null, SendMessageOptions.DontRequireReceiver);
			}
			if (parentPoolObj.sendPoolableActivateDeactivateMessages)
			{
				obj.SendMessage("OnPoolableObjectDeactivated", null, SendMessageOptions.DontRequireReceiver);
			}
			if (!recursiveCall)
			{
				_SetActive(obj, active: false);
			}
		}
	}

	internal static int _globalSerialNumber = 0;

	internal static bool _isDuringInstantiate = false;

	private static Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();

	public static bool isDuringPreload { get; private set; }

	public static GameObject Instantiate(GameObject prefab)
	{
		PoolableObject component = prefab.GetComponent<PoolableObject>();
		if (component == null)
		{
			return Object.Instantiate(prefab);
		}
		GameObject pooledInstance = _GetPool(component).GetPooledInstance(null, null);
		if ((bool)pooledInstance)
		{
			return pooledInstance;
		}
		return InstantiateWithoutPool(prefab);
	}

	public static GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion quaternion)
	{
		PoolableObject component = prefab.GetComponent<PoolableObject>();
		if (component == null)
		{
			return (GameObject)Object.Instantiate(prefab, position, quaternion);
		}
		GameObject pooledInstance = _GetPool(component).GetPooledInstance(position, quaternion);
		if ((bool)pooledInstance)
		{
			return pooledInstance;
		}
		return InstantiateWithoutPool(prefab, position, quaternion);
	}

	public static GameObject InstantiateWithoutPool(GameObject prefab)
	{
		return InstantiateWithoutPool(prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
	}

	public static GameObject InstantiateWithoutPool(GameObject prefab, Vector3 position, Quaternion quaternion)
	{
		_isDuringInstantiate = true;
		GameObject gameObject = (GameObject)Object.Instantiate(prefab, position, quaternion);
		_isDuringInstantiate = false;
		PoolableObject component = gameObject.GetComponent<PoolableObject>();
		if ((bool)component)
		{
			if (component.doNotDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(gameObject);
			}
			component._createdWithPoolController = true;
			Object.Destroy(component);
		}
		return gameObject;
	}

	public static void Destroy(GameObject obj)
	{
		PoolableObject component = obj.GetComponent<PoolableObject>();
		if (component == null)
		{
			_DetachChildrenAndDestroy(obj.transform);
			Object.Destroy(obj);
			return;
		}
		if (component._myPool != null)
		{
			component._myPool._SetAvailable(component, b: true);
			return;
		}
		if (!component._createdWithPoolController)
		{
			Debug.LogWarning("Poolable object " + obj.name + " not created with ObjectPoolController");
		}
		Object.Destroy(obj);
	}

	public static void Preload(GameObject prefab)
	{
		PoolableObject component = prefab.GetComponent<PoolableObject>();
		if (component == null)
		{
			Debug.LogWarning("Can not preload because prefab '" + prefab.name + "' is not poolable");
			return;
		}
		ObjectPool objectPool = _GetPool(component);
		int num = component.preloadCount - objectPool.GetObjectCount();
		if (num <= 0)
		{
			return;
		}
		isDuringPreload = true;
		try
		{
			for (int i = 0; i < num; i++)
			{
				objectPool.PreloadInstance();
			}
		}
		finally
		{
			isDuringPreload = false;
		}
	}

	internal static ObjectPool _GetPool(PoolableObject prefabPoolComponent)
	{
		GameObject gameObject = prefabPoolComponent.gameObject;
		if (!_pools.TryGetValue(gameObject, out var value))
		{
			value = new ObjectPool(gameObject);
			_pools.Add(gameObject, value);
		}
		return value;
	}

	private static void _DetachChildrenAndDestroy(Transform transform)
	{
		int childCount = transform.childCount;
		Transform[] array = new Transform[childCount];
		for (int i = 0; i < childCount; i++)
		{
			array[i] = transform.GetChild(i);
		}
		transform.DetachChildren();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = array[i].gameObject;
			if ((bool)gameObject)
			{
				Destroy(gameObject);
			}
		}
	}
}

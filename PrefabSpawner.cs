using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private Transform destination;

	[SerializeField]
	private Vector3 offset = Vector3.zero;

	[SerializeField]
	private bool parentParticles = true;

	public GameObject Spawn()
	{
		return spawnGO();
	}

	public GameObject SpawnAt(Vector3 position)
	{
		return SpawnAt(position, Quaternion.identity);
	}

	public GameObject SpawnAt(Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = spawnGO();
		gameObject.transform.parent = null;
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		return gameObject;
	}

	public T[] Spawn<T>() where T : Component
	{
		return Spawn().GetComponentsInChildren<T>(includeInactive: true);
	}

	public T[] SpawnAt<T>(Vector3 position) where T : Component
	{
		return SpawnAt(position).GetComponentsInChildren<T>(includeInactive: true);
	}

	public T SpawnOne<T>() where T : Component
	{
		T[] array = Spawn<T>();
		if (array.Length > 0)
		{
			return array[0];
		}
		return (T)null;
	}

	public T SpawnOneAt<T>(Vector3 position) where T : Component
	{
		T[] array = SpawnAt<T>(position);
		if (array.Length > 0)
		{
			return array[0];
		}
		return (T)null;
	}

	public void OverridePrefab(GameObject withPrefab)
	{
		prefab = withPrefab;
	}

	public void SetDestination(Transform destination)
	{
		this.destination = destination;
	}

	protected GameObject spawnGO()
	{
		GameObject gameObject = Object.Instantiate(prefab);
		gameObject.transform.parent = destination;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		if (!parentParticles)
		{
			gameObject.transform.parent = null;
		}
		gameObject.transform.position += offset;
		return gameObject;
	}
}

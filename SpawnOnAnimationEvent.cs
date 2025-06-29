using UnityEngine;

public class SpawnOnAnimationEvent : MonoBehaviour
{
	[SerializeField]
	private PrefabSpawner spawner;

	public void SpawnEvent(Object prefab)
	{
		spawner.OverridePrefab((GameObject)prefab);
		spawner.SpawnAt(base.transform.position, base.transform.rotation);
	}
}

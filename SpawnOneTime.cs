using UnityEngine;

public class SpawnOneTime : MonoBehaviour
{
	[SerializeField]
	private PrefabSpawner spawner;

	protected GameObject go;

	protected virtual void Start()
	{
		go = spawner.Spawn();
		if (base.transform.HasComponent<Animator>())
		{
			GetComponent<Animator>().Rebind();
		}
	}
}

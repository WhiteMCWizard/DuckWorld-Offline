using UnityEngine;

namespace SLAM.SceneSaver;

public class SceneImporter : MonoBehaviour
{
	[SerializeField]
	public Object[] sceneObjects;

	[SerializeField]
	[HideInInspector]
	private string[] _scenePaths;

	public string[] Scenes => _scenePaths;

	public GameObject[] SpawnedLevelObjects { get; protected set; }

	private void OnValidate()
	{
	}

	private void Awake()
	{
		SpawnedLevelObjects = new GameObject[base.transform.childCount];
		for (int i = 0; i < base.transform.childCount; i++)
		{
			SpawnedLevelObjects[i] = base.transform.GetChild(i).gameObject;
		}
	}
}

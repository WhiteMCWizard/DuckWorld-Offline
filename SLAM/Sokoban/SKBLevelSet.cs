using UnityEngine;

namespace SLAM.Sokoban;

public class SKBLevelSet : ScriptableObject
{
	[SerializeField]
	private GameObject[] levelPrefabs;

	[SerializeField]
	private float duration;

	public GameObject[] LevelPrefabs => levelPrefabs;

	public float Duration => duration;
}

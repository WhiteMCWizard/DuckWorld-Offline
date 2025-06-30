using System;
using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Sokoban;

public abstract class SokobanGameController : GameController
{
	[Serializable]
	public class SokobanLevelSetting : LevelSetting
	{
		[SerializeField]
		private GameObject[] levelPrefabs;

		[SerializeField]
		private float duration;

		public GameObject[] LevelPrefabs => levelPrefabs;

		public float Duration => duration;
	}

	public class MarkerCompletedEvent
	{
		public SKBMarker Marker;
	}

	public class MarkerRemovedEvent
	{
		public SKBMarker Marker;
	}

	[SerializeField]
	[Header("Sokoban properties")]
	private SokobanLevelSetting[] settings;

	[SerializeField]
	protected Vector3 cameraOffset = Vector3.zero;

	[SerializeField]
	private Transform levelRoot;

	[SerializeField]
	protected SKBAvatarController avatar;

	[SerializeField]
	private Vector3 levelOffset = Vector3.right;

	protected GameObject[] levelInstances;

	protected int completedMarkerCount;

	protected int targetMarkerCount;

	public int CurrentLevelIndex { get; protected set; }

	public int TotalLevelCount => SelectedLevel<SokobanLevelSetting>().LevelPrefabs.Length;

	public SKBAvatarController Avatar => avatar;

	public override LevelSetting[] Levels => settings;

	protected GameObject currentLevelInstance => levelInstances[CurrentLevelIndex];

	protected virtual void OnEnable()
	{
		GameEvents.Subscribe<MarkerCompletedEvent>(onMarkerCompleted);
		GameEvents.Subscribe<MarkerRemovedEvent>(onMarkerRemoved);
	}

	protected virtual void OnDisable()
	{
		GameEvents.Unsubscribe<MarkerCompletedEvent>(onMarkerCompleted);
		GameEvents.Unsubscribe<MarkerRemovedEvent>(onMarkerRemoved);
	}

	protected virtual void onMarkerCompleted(MarkerCompletedEvent obj)
	{
		completedMarkerCount++;
	}

	protected virtual void onMarkerRemoved(MarkerRemovedEvent obj)
	{
		completedMarkerCount--;
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		spawnAllTheLevels();
		Transform transform = currentLevelInstance.GetComponentInChildren<SKBSpawnPoint>().transform;
		avatar.transform.position = transform.position + Vector3.down * 0.5f;
		avatar.transform.rotation = transform.rotation;
		completedMarkerCount = 0;
		targetMarkerCount = currentLevelInstance.GetComponentsInChildren<SKBMarker>().Length;
	}

	protected override void OnExitStateRunning()
	{
		base.OnExitStateRunning();
		avatar.enabled = false;
	}

	public override void Finish(bool succes)
	{
		avatar.enabled = false;
		base.Finish(succes);
	}

	public void LevelCompleted()
	{
		GameObject oldLevelInstance = currentLevelInstance;
		CurrentLevelIndex++;
		if (CurrentLevelIndex >= levelInstances.Length)
		{
			Finish(succes: true);
			return;
		}
		completedMarkerCount = 0;
		targetMarkerCount = currentLevelInstance.GetComponentsInChildren<SKBMarker>().Length;
		StartCoroutine(animateToNextRoom(oldLevelInstance));
	}

	public void ResetCurrentLevel()
	{
		Vector3 position = currentLevelInstance.transform.position;
		UnityEngine.Object.Destroy(currentLevelInstance);
		GameObject gameObject = spawnLevel(CurrentLevelIndex);
		gameObject.transform.position = position;
		Transform transform = currentLevelInstance.GetComponentInChildren<SKBSpawnPoint>().transform;
		avatar.transform.position = transform.position + Vector3.down * 0.5f;
		avatar.transform.rotation = transform.rotation;
		completedMarkerCount = 0;
		targetMarkerCount = currentLevelInstance.GetComponentsInChildren<SKBMarker>().Length;
	}

	private void spawnAllTheLevels()
	{
		GameObject gameObject = null;
		levelInstances = new GameObject[TotalLevelCount];
		for (int i = 0; i < SelectedLevel<SokobanLevelSetting>().LevelPrefabs.Length; i++)
		{
			GameObject gameObject2 = spawnLevel(i);
			if (gameObject != null)
			{
				Vector3 position = gameObject2.GetComponentInChildren<SKBSpawnPoint>().transform.position;
				Vector3 zero = Vector3.zero;
				zero = ((!(gameObject.GetComponentInChildren<SKBLevelExit>() != null)) ? (levelOffset * i) : gameObject.GetComponentInChildren<SKBLevelExit>().transform.position);
				gameObject2.transform.position = zero - position + levelOffset;
			}
			gameObject = gameObject2;
		}
	}

	private GameObject spawnLevel(int levelIndex)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(SelectedLevel<SokobanLevelSetting>().LevelPrefabs[levelIndex]);
		gameObject.transform.parent = levelRoot;
		gameObject.transform.position = Vector3.zero;
		levelInstances[levelIndex] = gameObject;
		return gameObject;
	}

	protected abstract IEnumerator animateToNextRoom(GameObject oldLevelInstance);

	protected void toggleLights(GameObject gameObj, bool enabled)
	{
		Light[] componentsInChildren = gameObj.GetComponentsInChildren<Light>();
		foreach (Light light in componentsInChildren)
		{
			if (!light.name.EndsWith("_NL"))
			{
				StartCoroutine(fadeLight(light, (!enabled) ? 0f : 7.6f));
			}
		}
	}

	private IEnumerator fadeLight(Light light, float targetIntensity)
	{
		Stopwatch sw = new Stopwatch(0.5f);
		float start = light.intensity;
		while (!sw.Expired)
		{
			yield return null;
			light.intensity = Mathf.Lerp(start, targetIntensity, sw.Progress);
		}
	}
}

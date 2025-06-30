using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.Sokoban.PushTheCrate;

public class PushTheCrateGameController : SokobanGameController
{
	private const int POINTS_PER_SECONDS_LEFT = 10;

	[SerializeField]
	private AnimationCurve cameraAnimationCurve;

	[SerializeField]
	private float cameraAnimationDuration = 1f;

	public override int GameId => 8;

	public float CurrentScore => Mathf.Round((LevelDuration - ElapsedTime) * 10f);

	public float ElapsedTime { get; protected set; }

	public float LevelDuration => SelectedLevel<SokobanLevelSetting>().Duration;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("PTC_VICTORYWINDOW_SCORE_TIME_LEFT", (int)ElapsedTime, 10), (int)ElapsedTime * 10);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_BEAGLE BOY 1";

	public override string IntroTextKey => "PTC_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	public override void Play(LevelSetting setting)
	{
		base.Play(setting);
		StartCoroutine(animateStartGameCamera());
	}

	protected override void WhileStateRunning()
	{
		base.WhileStateRunning();
		if (avatar.enabled)
		{
			ElapsedTime += Time.deltaTime;
		}
		if (ElapsedTime >= LevelDuration)
		{
			Finish(succes: false);
		}
	}

	protected override void onMarkerCompleted(MarkerCompletedEvent obj)
	{
		base.onMarkerCompleted(obj);
		if (completedMarkerCount >= targetMarkerCount)
		{
			base.currentLevelInstance.GetComponentInChildren<SKBLevelExit>().OpenDoors();
		}
	}

	protected override void onMarkerRemoved(MarkerRemovedEvent obj)
	{
		base.onMarkerRemoved(obj);
		base.currentLevelInstance.GetComponentInChildren<SKBLevelExit>().CloseDoors();
	}

	protected override IEnumerator animateToNextRoom(GameObject oldLevelInstance)
	{
		avatar.enabled = false;
		toggleLights(base.currentLevelInstance, enabled: true);
		StartCoroutine(animateCameraToNextRoom());
		yield return StartCoroutine(avatar.MoveToTargetPosition(avatar.transform, base.currentLevelInstance.GetComponentInChildren<SKBSpawnPoint>().transform.position + Vector3.down * 0.5f));
		oldLevelInstance.GetComponentInChildren<SKBLevelExit>().CloseDoors();
		toggleLights(oldLevelInstance, enabled: false);
		avatar.enabled = true;
	}

	private IEnumerator animateCameraToNextRoom()
	{
		Vector3 startPos = Camera.main.transform.position;
		Vector3 targetPos = base.currentLevelInstance.GetComponentInChildren<SKBCameraAnchor>().transform.position + cameraOffset;
		Stopwatch sw = new Stopwatch(cameraAnimationDuration);
		while (!sw.Expired)
		{
			yield return new WaitForEndOfFrame();
			Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, cameraAnimationCurve.Evaluate(sw.Progress));
		}
	}

	private IEnumerator animateStartGameCamera()
	{
		avatar.enabled = false;
		Camera.main.transform.position = levelInstances.Last().GetComponentInChildren<SKBCameraAnchor>().transform.position + cameraOffset;
		for (int i = levelInstances.Length - 1; i >= 0; i--)
		{
			Vector3 startPos = Camera.main.transform.position;
			Vector3 targetPos = levelInstances[i].GetComponentInChildren<SKBCameraAnchor>().transform.position + cameraOffset;
			Stopwatch sw = new Stopwatch(cameraAnimationDuration);
			toggleLights(levelInstances[i], enabled: true);
			while (!sw.Expired)
			{
				yield return new WaitForEndOfFrame();
				Camera.main.transform.position = Vector3.Lerp(startPos, targetPos, sw.Progress);
			}
			if (i > 0)
			{
				toggleLights(levelInstances[i], enabled: false);
			}
		}
		avatar.enabled = true;
	}
}

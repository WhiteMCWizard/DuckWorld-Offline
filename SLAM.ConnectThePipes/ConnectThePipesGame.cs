using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class ConnectThePipesGame : GameController
{
	public class LevelCompletedEvent
	{
	}

	public class LevelFailedEvent
	{
	}

	public class WaterFlowStarted
	{
	}

	public class WaterFlowStopped
	{
	}

	public class PipeClickedEvent
	{
		public CTPPipe pipe;
	}

	public class CollectiblePickupEvent
	{
		public CTPCollectiblePipe Pipe;
	}

	public class CollectibleLostEvent
	{
		public CTPCollectiblePipe Pipe;
	}

	[Serializable]
	public class CTPLevelSetting : LevelSetting
	{
		public GameObject LevelRoot;

		public float Duration;
	}

	private const int pointsPerSecondLeft = 10;

	private const int pointsPerCollectible = 100;

	[SerializeField]
	private CTPLevelSpawner levelSpawner;

	[SerializeField]
	private float cameraAnimationTime;

	[SerializeField]
	private AnimationCurve cameraAnimationCurve;

	[SerializeField]
	private CTPWaterFlowManager waterFlowManager;

	[SerializeField]
	private CTPInputManager inputManager;

	[SerializeField]
	private CTPAvatarController avatarController;

	private int currentLevelIndex;

	private Alarm gameTimer;

	private int collectiblesPickupCount;

	[SerializeField]
	private float startWaterWaitTime = 1f;

	[SerializeField]
	private float levelInBetweenWaitTime = 3f;

	[SerializeField]
	private CTPLevelSetting[] settings;

	public override int GameId => 4;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTP_VICTORYWINDOW_SCORE_TIMELEFT_SCORE", Mathf.CeilToInt(gameTimer.TimeLeft), 10), Mathf.CeilToInt(gameTimer.TimeLeft) * 10);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTP_VICTORYWINDOW_SCORE_COLLECTIBLE_SCORE", collectiblesPickupCount, 100), collectiblesPickupCount * 100);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_SCROOGE";

	public override string IntroTextKey => "CTP_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	public override LevelSetting[] Levels => settings;

	public CTPLevelSetting CurrentSettings => SelectedLevel<CTPLevelSetting>();

	protected override void Start()
	{
		base.Start();
		inputManager.AreControlsLocked = true;
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Subscribe<LevelFailedEvent>(onLevelFailed);
		GameEvents.Subscribe<CollectiblePickupEvent>(onCollectiblePickedUp);
		GameEvents.Subscribe<CollectibleLostEvent>(onCollectibleLost);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<LevelCompletedEvent>(onLevelCompleted);
		GameEvents.Unsubscribe<LevelFailedEvent>(onLevelFailed);
		GameEvents.Unsubscribe<CollectiblePickupEvent>(onCollectiblePickedUp);
		GameEvents.Unsubscribe<CollectibleLostEvent>(onCollectibleLost);
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		foreach (Transform item in CurrentSettings.LevelRoot.transform.parent)
		{
			item.gameObject.SetActive(item == CurrentSettings.LevelRoot.transform);
		}
		Transform transform2 = CurrentSettings.LevelRoot.transform.FindChild("CameraAnchor");
		Camera.main.transform.position = transform2.position;
		Camera.main.transform.rotation = transform2.rotation;
		gameTimer = Alarm.Create();
		if (CurrentSettings.Duration > 0f)
		{
			gameTimer.StartCountdown(CurrentSettings.Duration, onTimeUp, destroyWhenFinished: false);
			OpenView<TimerView>().SetTimer(gameTimer);
		}
		avatarController.WarpTo(CurrentSettings.LevelRoot.transform.GetComponentInChildren<CTPBeginPipe>());
		inputManager.AreControlsLocked = false;
		StartCoroutine(animateCameraToLevel());
	}

	public override void Finish(bool succes)
	{
		gameTimer.Pause();
		inputManager.AreControlsLocked = true;
		base.Finish(succes);
	}

	private void onTimeUp()
	{
		if (!waterFlowManager.IsWaterFlowing)
		{
			StartWaterFlow();
		}
	}

	public void StartWaterFlow()
	{
		if (!waterFlowManager.IsWaterFlowing)
		{
			startWaterFlowSequence();
		}
	}

	private void startWaterFlowSequence()
	{
		avatarController.StartWaterFlow();
		waterFlowManager.StartWaterFlowFromBeginPipes(CurrentSettings.LevelRoot, startWaterWaitTime);
		inputManager.AreControlsLocked = true;
	}

	private void onLevelFailed(LevelFailedEvent obj)
	{
		if (gameTimer.TimeLeft <= 0f)
		{
			Finish(succes: false);
		}
		else
		{
			inputManager.AreControlsLocked = false;
		}
	}

	private void onLevelCompleted(LevelCompletedEvent obj)
	{
		Finish(succes: true);
	}

	private void onCollectiblePickedUp(CollectiblePickupEvent obj)
	{
		collectiblesPickupCount++;
	}

	private void onCollectibleLost(CollectibleLostEvent evt)
	{
		collectiblesPickupCount--;
	}

	private IEnumerator animateCameraToLevel()
	{
		gameTimer.Pause();
		if (currentLevelIndex > 0)
		{
			yield return new WaitForSeconds(levelInBetweenWaitTime);
		}
		Transform camAnchor = CurrentSettings.LevelRoot.transform.FindChild("CameraAnchor");
		Vector3 startPos = Camera.main.transform.position;
		Vector3 endPos = camAnchor.position;
		Quaternion startRot = Camera.main.transform.rotation;
		Quaternion endRot = camAnchor.rotation;
		Stopwatch sw = new Stopwatch(cameraAnimationTime);
		while (!sw.Expired)
		{
			yield return null;
			Camera.main.transform.position = Vector3.Lerp(startPos, endPos, cameraAnimationCurve.Evaluate(sw.Progress));
			Camera.main.transform.rotation = Quaternion.Lerp(startRot, endRot, cameraAnimationCurve.Evaluate(sw.Progress));
		}
		gameTimer.Resume();
	}
}

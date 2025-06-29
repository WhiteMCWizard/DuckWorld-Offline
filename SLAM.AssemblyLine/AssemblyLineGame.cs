using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.AssemblyLine;

public class AssemblyLineGame : GameController
{
	[Serializable]
	public class AssemblyLineGameDifficulty : LevelSetting
	{
		[Tooltip("How many robots should be assembled")]
		public int RequiredRobotCount;

		[Tooltip("At what time interval do the parts spawn")]
		public float SpawnInterval;

		[Tooltip("How long does it take for a part to reach the end")]
		public float Duration;

		[Tooltip("What kind are allowed")]
		public int[] AllowedKinds;

		[Tooltip("The part the user deserves, not the one that the user needs")]
		public AnimationCurve RandomPartChanceCurve;

		[Tooltip("Visual speed of the belt material (no effect on gameplay)")]
		public float BeltMaterialSpeed;

		public int Lives;

		public int BeamCount = 3;
	}

	public class LifeLostEvent
	{
		public ALRobotPart Part;
	}

	public class RobotCompletedEvent
	{
		public ALDropZone DropZone;

		public GameObject Robot;
	}

	public class PartHoverEvent
	{
	}

	public class PartDroppedEvent
	{
		public ALRobotPart Part;

		public ALDropZone DropZone;
	}

	public class PartPickedUpEvent
	{
		public ALRobotPart Part;
	}

	public class PartReleasedEvent
	{
		public ALRobotPart Part;
	}

	public class PartSpawnedEvent
	{
		public ALRobotPart Part;
	}

	public const int POINTS_ROBOTCOMPLETED = 1;

	public const int POINTS_LIVES_LEFT = 1;

	[SerializeField]
	[Header("Assembly Line properties")]
	private AssemblyLineGameDifficulty[] difficultySettings;

	[SerializeField]
	private ALDragAndDropManager dragdropManager;

	[SerializeField]
	private ALConveyorBelt conveyorBelt;

	[SerializeField]
	private ALDropZone[] dropZones;

	private bool finishedGame;

	public int RequiredRobotCount { get; protected set; }

	public int CompletedRobots { get; protected set; }

	public int Lives { get; protected set; }

	public int PartsDropped { get; protected set; }

	public override LevelSetting[] Levels => difficultySettings;

	protected AssemblyLineGameDifficulty currentDifficultySetting => SelectedLevel<AssemblyLineGameDifficulty>();

	public override int GameId => 12;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("UI_VICTORYWINDOW_COINS_EARNED", getCoinRewardForThisLevel()), getCoinRewardForThisLevel());
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_GYRO_GEARLOOSE";

	public override string IntroTextKey => "AL_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.GyroGearloose;

	private void OnEnable()
	{
		GameEvents.Subscribe<LifeLostEvent>(onLifeLost);
		GameEvents.Subscribe<PartDroppedEvent>(onPartDropped);
		GameEvents.Subscribe<RobotCompletedEvent>(onRobotCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<LifeLostEvent>(onLifeLost);
		GameEvents.Unsubscribe<PartDroppedEvent>(onPartDropped);
		GameEvents.Unsubscribe<RobotCompletedEvent>(onRobotCompleted);
	}

	private void onPartDropped(PartDroppedEvent evt)
	{
		PartsDropped++;
	}

	private void onLifeLost(LifeLostEvent evt)
	{
		GetView<HeartsView>().LoseHeart();
		if (--Lives <= 0)
		{
			CoroutineUtils.WaitForObjectDestroyed(evt.Part.gameObject, delegate
			{
				Finish(success: false);
			});
		}
	}

	private void onRobotCompleted(RobotCompletedEvent evt)
	{
		CompletedRobots = Mathf.Min(CompletedRobots + 1, currentDifficultySetting.RequiredRobotCount);
		if (base.StateMachine.CurrentState.Name == "Running" && CompletedRobots >= RequiredRobotCount && !finishedGame)
		{
			finishedGame = true;
			conveyorBelt.PauseSpawningParts();
			StartCoroutine(waitForFinish(evt.Robot.gameObject));
		}
	}

	private IEnumerator waitForFinish(GameObject part)
	{
		while (part != null)
		{
			yield return null;
		}
		yield return new WaitForSeconds(1.5f);
		Finish(success: true);
	}

	protected override void Start()
	{
		base.Start();
		conveyorBelt.enabled = false;
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		for (int i = 0; i < dropZones.Length; i++)
		{
			dropZones[i].gameObject.SetActive(i < currentDifficultySetting.BeamCount);
		}
		RequiredRobotCount = currentDifficultySetting.RequiredRobotCount;
		conveyorBelt.SetDifficulty(currentDifficultySetting);
		conveyorBelt.enabled = true;
		CompletedRobots = 0;
		Lives = currentDifficultySetting.Lives;
		OpenView<HeartsView>().SetTotalHeartCount(Lives);
		GetView<ALHudView>().InitHUD(RequiredRobotCount);
	}

	protected override void OnExitStateRunning()
	{
		base.OnExitStateRunning();
		conveyorBelt.enabled = false;
	}

	public override void Pause()
	{
		base.Pause();
		dragdropManager.enabled = false;
	}

	public override void Resume()
	{
		base.Resume();
		dragdropManager.enabled = true;
	}

	public override void Finish(bool success)
	{
		base.Finish(success);
		dragdropManager.enabled = false;
	}
}

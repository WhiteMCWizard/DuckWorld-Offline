using System;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MonkeyBattleGame : GameController
{
	[Serializable]
	public class MB_Settings : LevelSetting
	{
		[Header("Generic settings")]
		public float heartRespawnInterval = 10f;

		[Header("Flintheart settings")]
		public float flintheartReloadTime;

		public float flintheartBananaDamage;

		public int flintheartShootCount;

		public float flintheartShootIntervalMin;

		public float flintheartShootIntervalMax;

		public bool flintheartDoCinematicTierSwitches;

		[Header("Monkey settings")]
		public float monkeySwingRotationSpeed;

		public float monkeyFireIntervalMin;

		public float monkeyFireIntervalMax;

		[Header("Turret settings")]
		public float turretAppearanceIntervalMin;

		public float turretAppearanceIntervalMax;

		public float turretShootTimeMax;

		[Header("Shoot settings")]
		public ShootActions[] shotPattern;

		public float sprayShotCount = 5f;

		public float sprayShotAngle = 20f;

		public float sprayShotInterval = 0.37f;

		public float buckShotCount = 5f;

		public float buckShotAngle = 40f;

		public float followShotDuration = 4f;

		public float followShotInterval = 0.15f;

		public float followShotAmplitude = 3f;

		public float followShotFrequency = 2f;

		public float shootInFrontDuration = 2f;
	}

	[Serializable]
	public enum ShootActions
	{
		Reloading,
		SingleShot,
		BuckShot,
		SprayShot,
		FollowShot,
		AngleShot
	}

	public class TurretEnteredEvent
	{
		public MB_Turret turret;
	}

	public class TurretExitedEvent
	{
		public MB_Turret turret;
	}

	public class GameStartedEvent
	{
		public MB_Settings settings;
	}

	public class GameEndedEvent
	{
		public bool Success;
	}

	public class FlintheartStartedReloadingEvent
	{
		public MB_Flintheart.FlintheartTier Spot;
	}

	public class FlintheartFinishedReloadingEvent
	{
	}

	public class FlintheartHitEvent
	{
		public MB_Banana Banana;
	}

	public class MonkeyHitEvent
	{
		public MB_Monkey Monkey;

		public MB_Banana Banana;
	}

	public class HeartPickedUp
	{
		public MB_HeartPickup Pickup;
	}

	[SerializeField]
	private MB_Settings[] settingsPerDifficulty;

	[SerializeField]
	private MB_PlayerController playerController;

	[SerializeField]
	private MB_Flintheart flintheartController;

	private bool hasFinished;

	private int hearts = 3;

	public override LevelSetting[] Levels => settingsPerDifficulty;

	public override int GameId => 28;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_HEARTS_LEFT", hearts, 100), hearts * 100);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "MB_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	private void OnEnable()
	{
		GameEvents.Subscribe<PlayerHitEvent>(onPlayerHit);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<PlayerHitEvent>(onPlayerHit);
	}

	protected override void Start()
	{
		base.Start();
		playerController.AreControlsLocked = true;
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		GameStartedEvent gameStartedEvent = new GameStartedEvent();
		gameStartedEvent.settings = SelectedLevel<MB_Settings>();
		GameEvents.Invoke(gameStartedEvent);
		playerController.AreControlsLocked = false;
		OpenView<HeartsView>().SetTotalHeartCount(hearts);
	}

	public override void Finish(bool success)
	{
		if (!hasFinished)
		{
			hasFinished = true;
			GameEndedEvent gameEndedEvent = new GameEndedEvent();
			gameEndedEvent.Success = success;
			GameEvents.Invoke(gameEndedEvent);
			playerController.AreControlsLocked = true;
			playerController.GetComponent<Animator>().SetBool("hasDied", !success);
			flintheartController.StopAllCoroutines();
			base.Finish(success);
		}
	}

	private void onPlayerHit(PlayerHitEvent obj)
	{
		if (!(flintheartController.Health <= 0f))
		{
			if (--hearts <= 0)
			{
				Finish(success: false);
			}
			else
			{
				GetView<HeartsView>().LoseHeart();
			}
		}
	}

	public bool CollectHeart(MB_HeartPickup pickup)
	{
		if (hearts < 3)
		{
			hearts++;
			GetView<HeartsView>().FoundHeart(pickup.transform.position);
			return true;
		}
		return false;
	}
}

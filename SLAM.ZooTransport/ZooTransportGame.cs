using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.ZooTransport;

public class ZooTransportGame : GameController
{
	private enum PrespawnState
	{
		Mistake,
		Honk,
		Cheer
	}

	public class ZTGameEndEvent
	{
	}

	public class ZTTruckReachedEndEevent
	{
		public Vector3 TruckEndPosition;
	}

	public class ZTCountdownFinishedEvent
	{
	}

	[Serializable]
	public class ZTLevelSetting : LevelSetting
	{
		public float gameDuration;
	}

	private const int POINTS_PER_CRATE = 100;

	private const int POINTS_PER_SECONDS_LEFT = 10;

	public const string STATE_PHASE1 = "Phase1";

	public const string STATE_PRESPAWN = "Prespawn";

	public const string STATE_RESPAWN = "Respawn";

	public static ZooTransportGame instance;

	[SerializeField]
	[Header("Zoo Transport properties")]
	private float prespawnTime;

	[SerializeField]
	private float respawnTime;

	[SerializeField]
	private ZTtruck truck;

	[SerializeField]
	private ZTlevelController levelController;

	[SerializeField]
	private ZTcargoCheck cargo;

	[SerializeField]
	private Animator avatar;

	[SerializeField]
	private ZTcameraManager camManager;

	[SerializeField]
	private AudioClip tootSound;

	[SerializeField]
	private AudioClip gameOverMusic;

	[SerializeField]
	private float scroogeCheerWaitTime;

	private float spawnTimer;

	private PrespawnState prespawnState;

	private Alarm levelTimer;

	private int lives = 3;

	[SerializeField]
	private ZTLevelSetting[] settings;

	public override int GameId => 9;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			int num = ((!(levelTimer == null)) ? ((int)levelTimer.TimeLeft) : 0);
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("ZT_VICTORYWINDOW_SCORE_TIME_LEFT", num, 10), num * 10);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("ZT_VICTORYWINDOW_SCORE_CRATES_DELIVERED", cargo.cratesInLevel, 100), cargo.cratesInLevel * 100);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "ZT_CINEMATICINTRO_TEXT";

	public override LevelSetting[] Levels => settings;

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	public ZTLevelSetting CurrentSettings => SelectedLevel<ZTLevelSetting>();

	protected override void Start()
	{
		base.Start();
		instance = this;
		levelController.SetCargo(cargo);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<ZTCountdownFinishedEvent>(onCountdownFinished);
		GameEvents.Subscribe<ZTGameEndEvent>(onGameEnd);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<ZTCountdownFinishedEvent>(onCountdownFinished);
		GameEvents.Unsubscribe<ZTGameEndEvent>(onGameEnd);
	}

	private void onCountdownFinished(ZTCountdownFinishedEvent evt)
	{
		if (!IsViewOpen<TimerView>() && CurrentSettings.gameDuration > 0f)
		{
			levelTimer = Alarm.Create();
			levelTimer.StartCountdown(CurrentSettings.gameDuration * 60f, delegate
			{
				Finish(succes: false);
			});
			OpenView<TimerView>().SetTimer(levelTimer);
		}
	}

	private void onGameEnd(ZTGameEndEvent evt)
	{
		StartCoroutine(doEndOfLevel());
	}

	private void FixedUpdate()
	{
		if (base.StateMachine.CurrentState != null && base.StateMachine.CurrentState.Name == "Phase1")
		{
			truck.ManualUpdate();
			cargo.ManualUpdate();
		}
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		levelController.SetDifficulty(SelectedLevel<LevelSetting>().Index);
		OpenView<HeartsView>().SetTotalHeartCount(lives);
		base.StateMachine.SwitchTo("Respawn");
	}

	private void Prespawn(PrespawnState state)
	{
		if (!(base.StateMachine.CurrentState.Name == "Prespawn"))
		{
			if (state == PrespawnState.Mistake)
			{
				GetView<HeartsView>().LoseHeart();
				lives--;
			}
			prespawnState = state;
			base.StateMachine.SwitchTo("Prespawn");
		}
	}

	private void PreSpawnInit()
	{
		spawnTimer = 0f;
		camManager.CrossFade(1);
		StartCoroutine(doPreSpawn());
	}

	private IEnumerator doPreSpawn()
	{
		yield return StartCoroutine(truck.DoStopTruck());
		camManager.CrossFade(2);
		avatar.SetTrigger(prespawnState.ToString());
		yield return new WaitForSeconds(tootSound.length * 2f);
		FinishPreSpawn();
	}

	private void WhilePrespawn()
	{
	}

	private void FinishPreSpawn()
	{
		if (prespawnState == PrespawnState.Cheer)
		{
			Finish(succes: true);
			return;
		}
		if (lives <= 0)
		{
			Finish(succes: false);
			return;
		}
		OpenView<FadeView>(delegate
		{
			base.StateMachine.SwitchTo("Respawn");
		});
	}

	private void RespawnInit()
	{
		truck.transform.position = new Vector3(0.295f, 0.095f, 0f);
		truck.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		Rigidbody component = truck.GetComponent<Rigidbody>();
		if (!component.isKinematic)
		{
			component.velocity = Vector3.zero;
			component.isKinematic = true;
		}
		levelController.LoadLevel(SelectedLevel<LevelSetting>().Index);
		spawnTimer = 0f;
		avatar.SetTrigger("Idle");
		camManager.WarpTo();
		GetView<ZThudView>().DoCountdown();
		if (!GetView<FadeView>().IsOpen)
		{
			camManager.CrossFade(0);
			return;
		}
		CloseView<FadeView>(delegate
		{
			camManager.CrossFade(0);
		});
	}

	private void WhileRespawn()
	{
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= respawnTime)
		{
			base.StateMachine.SwitchTo("Phase1");
		}
	}

	private void FinishRespawn()
	{
		truck.GetComponent<Rigidbody>().isKinematic = false;
		truck.enabled = true;
	}

	protected override void AddStates()
	{
		base.AddStates();
		base.StateMachine.AddState("Phase1", onPhase1Enter, null, onPhase1Exit);
		base.StateMachine.AddState("Prespawn", PreSpawnInit, WhilePrespawn, null);
		base.StateMachine.AddState("Respawn", RespawnInit, WhileRespawn, FinishRespawn);
	}

	private void onPhase1Enter()
	{
		if (levelTimer != null)
		{
			levelTimer.Resume();
		}
	}

	private void onPhase1Exit()
	{
		if (levelTimer != null)
		{
			levelTimer.Pause();
		}
	}

	private IEnumerator doEndOfLevel()
	{
		yield return new WaitForSeconds(scroogeCheerWaitTime);
		int cratesCheck = cargo.CheckCargo();
		if (cratesCheck >= 0)
		{
			Prespawn(PrespawnState.Cheer);
		}
		else
		{
			Prespawn(PrespawnState.Mistake);
		}
	}

	public void GotCrate()
	{
	}

	public void LostCrate()
	{
		if (!truck.HasReachedEnd)
		{
			Prespawn(PrespawnState.Mistake);
		}
	}

	public void TumbledDown()
	{
		base.StateMachine.SwitchTo("Prespawn");
	}
}

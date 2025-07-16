using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.CameraSystem;
using SLAM.Engine;
using SLAM.Kart;
using SLAM.Kartshop;
using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.KartRacing;

public class KR_GameController : GameController
{
	private const string STATE_READY_TO_RACE = "Ready to race";

	private const string STATE_RACING = "Racing";

	[SerializeField]
	private KR_TrackInfo[] tracks;

	[SerializeField]
	private float countdownDuration = 3f;

	[SerializeField]
	private float finishToResultscreenDelay = 3f;

	[SerializeField]
	private CameraManager cameraManager;

	[SerializeField]
	private KRgameCamera gameCamBehaviour;

	[SerializeField]
	private GameObject humanKartPrefab;

	[SerializeField]
	private GameObject aiKartPrefab;

	[SerializeField]
	private GameObject ghostKartPrefab;

	private KR_Track selectedTrack;

	private KR_KartBase[] allKarts;

	private GhostRecordingData ghostData;

	public override LevelSetting[] Levels => tracks;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			throw new NotImplementedException("Kartracing uses a different resultview, without categories!");
		}
	}

	public override int TotalScore => (int)TimeSpan.FromSeconds(HumanKart.Timer.CurrentTime).TotalMilliseconds;

	public override string IntroNPCKey => "NPC_NAME_HUEY";

	public override string IntroTextKey => "KR_CINEMATICINTRO_TEXT";

	public override int GameId => 11;

	private KR_HumanKart HumanKart => (KR_HumanKart)allKarts[0];

	public override Portrait DuckCharacter => Portrait.GrandMogul;

	protected override void AddStates()
	{
		base.AddStates();
		base.StateMachine.AddState("Ready to race", OnEnterStateReadyToRace, null, null);
		base.StateMachine.AddState("Racing", OnEnterStateRacing, WhileStateRacing, null);
	}

	protected override void Start()
	{
		if (KartSystem.PlayerKartConfiguration == null)
		{
			KSShop.GetUserKart(delegate(KartConfigurationData config)
			{
				KartSystem.PlayerKartConfiguration = config;
				base.Start();
			});
		}
		else
		{
			base.Start();
		}
	}

	public override void Play(LevelSetting setting)
	{
		base.Play(setting);
		KR_TrackInfo settings = SelectedLevel<KR_TrackInfo>();
		selectLevel(settings);
	}

	protected override void OnEnterStateRunning()
	{
		base.OnEnterStateRunning();
		base.StateMachine.SwitchTo("Ready to race");
	}

	private void OnEnterStateReadyToRace()
	{
		KR_TrackInfo kR_TrackInfo = SelectedLevel<KR_TrackInfo>();
		int userId = ApiClient.UserId;
		string difficulty = kR_TrackInfo.Difficulty;
		if (kR_TrackInfo.mode == KRGameMode.Time)
		{
			if (IsViewOpen<StartView>() && kR_TrackInfo.mode == KRGameMode.Time)
			{
				CloseView<StartView>();
			}
			if (GameController.ChallengeAccepted != null)
			{
				userId = GameController.ChallengeAccepted.Sender.Id;
				difficulty = SelectedLevel<LevelSetting>().Difficulty;
			}
			int difficultyInt;
			if (!int.TryParse(difficulty, out difficultyInt))
			{
				difficultyInt = 0; // fallback if parsing fails
			}
			SaveManager.Instance.GetBestGhost(userId, difficultyInt, delegate(GhostRecordingData recording)
			{
				if (!Equals(recording, default(GhostRecordingData)))
				{
					ghostData = recording;
					base.StateMachine.SwitchTo("Racing");
				}
				else
				{
					base.StateMachine.SwitchTo("Racing");
				}
			});
		}
		else
		{
			base.StateMachine.SwitchTo("Racing");
		}
	}

	private void OnEnterStateRacing()
	{
		KR_TrackInfo settings = SelectedLevel<KR_TrackInfo>();
		allKarts = spawnKarts(settings);
		gameCamBehaviour.SetTarget(HumanKart.transform);
		cameraManager.CrossFade(gameCamBehaviour, 0f);
		gameCamBehaviour.WarpTo();
		StartCoroutine(doCountdownRoutine(countdownDuration));
	}

	private void WhileStateRacing()
	{
		List<KR_KartBase> list = new List<KR_KartBase>(allKarts);
		list.Sort((KR_KartBase a, KR_KartBase b) => b.TrackProgress.CompareTo(a.TrackProgress));
		for (int num = 0; num < list.Count; num++)
		{
			list[num].RacePosition = num + 1;
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<KR_FinishCrossedEvent>(onFinishCrossed);
		GameEvents.Subscribe<KR_GameOverEvent>(onGameOver);
		GameEvents.Subscribe<KR_HumanHeartCountChangedEvent>(onHumanHeartCountChanged);
		GameEvents.Subscribe<KR_HumanRacePositionChangedEvent>(onHumanRacePositionChanged);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<KR_FinishCrossedEvent>(onFinishCrossed);
		GameEvents.Unsubscribe<KR_GameOverEvent>(onGameOver);
		GameEvents.Unsubscribe<KR_HumanHeartCountChangedEvent>(onHumanHeartCountChanged);
		GameEvents.Unsubscribe<KR_HumanRacePositionChangedEvent>(onHumanRacePositionChanged);
	}

	public override void Finish(bool succes)
	{
		base.StateMachine.SwitchTo("Finished");
	}

	private void selectLevel(KR_TrackInfo settings)
	{
		for (int i = 0; i < tracks.Length; i++)
		{
			tracks[i].trackObject.SetActive(value: false);
		}
		GameObject trackObject = settings.trackObject;
		trackObject.SetActive(value: true);
		Transform trackRoot = trackObject.transform.FindChildRecursively("Waypoints");
		selectedTrack = new KR_Track(trackRoot);
		AudioController.StopCategory("Ambience", 0f);
		AudioController.Play(settings.ambienceAudio.name);
	}

	private void onFinishCrossed(KR_FinishCrossedEvent evt)
	{
		Transform transform = SelectedLevel<KR_TrackInfo>().trackObject.transform.FindChildRecursively("Finishpoints");
		if (evt.PodiumPosition > transform.childCount)
		{
			Debug.Log("can't find finishPos " + (evt.PodiumPosition - 1));
		}
		evt.Kart.DriveToLocation(transform.GetChild(evt.PodiumPosition - 1));
		GetView<KRresultView>().AddFinishedKart(evt.Kart, evt.PodiumPosition);
		if (!(evt.Kart is KR_HumanKart))
		{
			return;
		}
		closeHud();
		bool flag = evt.PodiumPosition == 1;
		KR_HumanKart kR_HumanKart = evt.Kart as KR_HumanKart;
		int trackTimeInMilisec = TotalScore;
		kR_HumanKart.StopAudio();
		AudioController.Play((!flag) ? "KR_player_loses" : "KR_player_wins");
		DataStorage.GetLocationsData(delegate(Location[] locs)
		{
			GameEvents.Invoke(new TrackingEvent
			{
				Type = TrackingEvent.TrackingType.GameWon,
				Arguments = new Dictionary<string, object>
				{
					{ "GameId", GameId },
					{
						"Difficulty",
						SelectedLevel<LevelSetting>().Difficulty
					},
					{
						"Progress",
						(float)(SelectedLevel<LevelSetting>().Index + 1) / (float)Levels.Length
					},
					{
						"Time",
						Time.timeSinceLevelLoad
					},
					{ "Score", trackTimeInMilisec },
					{ "Coins", 0 },
					{
						"LocationName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == GameId)).Name
					},
					{
						"GameName",
						locs.FirstOrDefault((Location l) => l.Games.Any((Game loc) => loc.Id == GameId)).GetGame(GameId).Name
					}
				}
			});
		});
		StartCoroutine(waitAndOpenResults(flag));
		bool gameCompleted = !UserProfile.Current.IsFree && SelectedLevel<LevelSetting>().Index >= Levels.Length - 1;
		if ((SelectedLevel<KR_TrackInfo>().mode == KRGameMode.Race && (flag || GameController.ChallengeAccepted != null)) || SelectedLevel<KR_TrackInfo>().mode == KRGameMode.Time)
		{
			SaveManager.Instance.GetSaveData().SaveScore(GameId, trackTimeInMilisec, SelectedLevel<LevelSetting>().Difficulty, trackTimeInMilisec, gameCompleted, base.onScoresSubmitted);
		}
	}

	private IEnumerator waitAndOpenResults(bool isHumanWinner)
	{
		yield return new WaitForSeconds(finishToResultscreenDelay);
		OpenView<KRresultView>().ShowResult(isHumanWinner, currentGameInfo, SelectedLevel<KR_TrackInfo>());
	}

	private void onGameOver(KR_GameOverEvent evt)
	{
		closeHud();
		OpenView<FailedView>().SetInfo(SelectedLevel<KR_TrackInfo>());
		AudioController.Play("KR_player_loses");
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.GameLost;
		trackingEvent.Arguments = new Dictionary<string, object>
		{
			{ "GameId", GameId },
			{
				"Difficulty",
				SelectedLevel<LevelSetting>().Difficulty
			},
			{
				"Time",
				Time.timeSinceLevelLoad
			},
			{ "LocationName", currentLocationInfo.Name },
			{ "GameName", currentGameInfo.Name }
		};
		GameEvents.Invoke(trackingEvent);
	}

	private void onHumanHeartCountChanged(KR_HumanHeartCountChangedEvent evt)
	{
		HeartsView view = GetView<HeartsView>();
		if (evt.OldHeartCount > evt.NewHeartCount)
		{
			view.LoseHeart();
		}
		else if (evt.OldHeartCount < evt.NewHeartCount)
		{
			view.FoundHeart(evt.Kart.transform.position);
		}
	}

	private void onHumanRacePositionChanged(KR_HumanRacePositionChangedEvent evt)
	{
		KRhudView view = GetView<KRhudView>();
		view.UpdatePosition(evt.NewRacePosition, allKarts.Length);
	}

	private IEnumerator doCountdownRoutine(float duration)
	{
		CloseView<HUDView>();
		KRcountdownView countdownView = OpenView<KRcountdownView>();
		countdownView.BeginCountdown(duration);
		yield return new WaitForSeconds(duration);
		KR_StartRaceEvent kR_StartRaceEvent = new KR_StartRaceEvent();
		kR_StartRaceEvent.GameId = GameId;
		kR_StartRaceEvent.Track = selectedTrack;
		kR_StartRaceEvent.Karts = allKarts;
		kR_StartRaceEvent.TrackIndex = SelectedLevel<LevelSetting>().Index + 1;
		GameEvents.Invoke(kR_StartRaceEvent);
		CloseView<KRcountdownView>();
		OpenView<HUDView>();
		OpenView<HeartsView>().SetTotalHeartCount(HumanKart.HeartsLeft);
		OpenView<TimerView>().SetTimer(HumanKart.Timer);
	}

	private void closeHud()
	{
		CloseView<TimerView>();
		CloseView<KRhudView>();
		CloseView<HeartsView>();
	}

	private KR_KartBase[] spawnKarts(KR_TrackInfo settings)
	{
		List<KR_KartBase> list = new List<KR_KartBase>();
		GameObject gameObject = UnityEngine.Object.Instantiate(humanKartPrefab);
		list.Add(gameObject.GetComponent<KR_HumanKart>());
		if (settings.mode == KRGameMode.Race)
		{
			for (int i = 0; i < settings.AISettings.Length; i++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(aiKartPrefab);
				KR_ComputerKart component = gameObject2.GetComponent<KR_ComputerKart>();
				list.Add(component);
				KR_AISettings aiSettings = settings.AISettings[i];
				component.ApplyAISettings(aiSettings);
			}
		}
		gameObject.AddComponent<KR_GhostRecorder>();
		if (ghostData.Records != null)
		{
			GameObject gameObject3 = UnityEngine.Object.Instantiate(ghostKartPrefab);
			KR_GhostKart component2 = gameObject3.GetComponent<KR_GhostKart>();
			component2.SetRecording(ghostData, (GameController.ChallengeAccepted == null) ? component2.PlayerName : GameController.ChallengeAccepted.Sender.Name);
			list.Add(gameObject3.GetComponent<KR_GhostKart>());
		}
		int num = 0;
		foreach (KR_KartBase item in list)
		{
			if (!(item is KR_GhostKart))
			{
				Transform transform = SelectedLevel<KR_TrackInfo>().trackObject.transform.FindChildRecursively("Spawnpoint" + num);
				list[num].transform.position = transform.position;
				list[num].transform.rotation = transform.rotation;
				num++;
			}
		}
		return list.ToArray();
	}
}

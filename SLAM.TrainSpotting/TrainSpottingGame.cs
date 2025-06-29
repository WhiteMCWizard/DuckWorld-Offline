using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TrainSpottingGame : GameController
{
	public enum TimeMode
	{
		Analog,
		Digital12,
		Digital24
	}

	[Flags]
	public enum TimeSets
	{
		Hours = 1,
		Half = 2,
		Quarter = 4,
		Minutes = 8
	}

	[Serializable]
	public class TSDifficultySettings : LevelSetting
	{
		public int TargetTrainCount;

		public TimeMode timeMode;

		[BitMask(typeof(TimeSets))]
		public TimeSets timeSet;

		public float TimeScale;

		public float TrainIntervalMin = 3f;

		public float TrainIntervalMax = 10f;

		[Tooltip("Time range in minutes how quickly after spawning we have to depart. In TrainSpottingTime")]
		public float TrainDepartureTimeMin = 10f;

		[Tooltip("Time range in minutes how quickly after spawning we have to depart. In TrainSpottingTime")]
		public float TrainDepartureTimeMax = 100f;

		public int MaxActiveTrainsCount = 4;

		[Tooltip("How much room for error there is, in minutes.")]
		public float TrainDepartureThreshold = 2f;
	}

	public class TrainInfo
	{
		public TimeMode TimeMode;

		public string Destination;

		public float TargetDepartureTime;

		public bool TrainIsDeparted;

		public TSTrainTrack Track;

		public TSTrain TrainObject;
	}

	public class GameStartedEvent
	{
	}

	public class GameFinishedEvent
	{
	}

	public class TrainShouldDepartEvent
	{
		public TSScheduleItemView scheduleItem;

		public TrainInfo trainInfo;
	}

	public class TrainQueuedEvent
	{
		public TrainInfo TrainInfo;

		public float QueueEnterTime;
	}

	public class TrainArrivedEvent
	{
		public TrainInfo TrainInfo;

		public TSTrainTrack Track;

		public float ArrivalTime;
	}

	public class TrainDepartedEvent
	{
		public TrainInfo TrainInfo;

		public float DepartureTime;

		public bool WasOnTime;
	}

	public class TrainPassedByEvent
	{
		public TrainInfo TrainInfo;
	}

	public class TrainScheduleItemClickedEvent
	{
		public TSScheduleItemView ScheduleItem;
	}

	public class TrainTrackClickedEvent
	{
		public TSTrainTrack Track;
	}

	public class MessageEvent
	{
		public string Message;
	}

	public class CrowdEnteredTrain
	{
		public TrainInfo TrainInfo;
	}

	public const int CORRECT_TRAIN_SCORE = 10;

	public const int INCORRECT_TRAIN_SCORE = -20;

	private int hearts = 3;

	[Header("Trainspotting Properties")]
	[SerializeField]
	private TSCrowdManager crowdManager;

	[SerializeField]
	private TSDifficultySettings[] settings;

	public int CrowdCount;

	private string[] trainDestinations;

	private int activeTrains;

	private float gameStartTime;

	private float nextTrainQueueTime;

	private int correctTrains;

	private int incorrectTrains;

	private bool areTrainsPaused;

	public override LevelSetting[] Levels => settings;

	public TSDifficultySettings CurrentSettings => SelectedLevel<TSDifficultySettings>();

	public float AbsoluteElapsedTime => ElapsedTime + 36000f;

	public float ElapsedTime => (Time.time - gameStartTime) * CurrentSettings.TimeScale;

	public override int GameId => 30;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("UI_VICTORYWINDOW_COINS_EARNED", getCoinRewardForThisLevel()), getCoinRewardForThisLevel());
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_SCROOGE";

	public override string IntroTextKey => "TS_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Subscribe<TrainArrivedEvent>(onTrainArrived);
		GameEvents.Subscribe<TrainPassedByEvent>(onTrainPassedBy);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Unsubscribe<TrainArrivedEvent>(onTrainArrived);
		GameEvents.Unsubscribe<TrainPassedByEvent>(onTrainPassedBy);
	}

	protected override void Start()
	{
		base.Start();
		trainDestinations = Localization.Get("TS_DESTINATIONS").Split(',');
		GetComponent<TSSchedulingManager>().enabled = false;
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		gameStartTime = Time.time;
		GetComponent<TSSchedulingManager>().enabled = true;
		OpenView<TSScheduleView>();
		OpenView<HeartsView>().SetTotalHeartCount(hearts);
		GameEvents.Invoke(new GameStartedEvent());
	}

	public void PauseTrains()
	{
		areTrainsPaused = true;
	}

	public void ResumeTrains()
	{
		areTrainsPaused = false;
	}

	protected override void WhileStateRunning()
	{
		base.WhileStateRunning();
		if (ElapsedTime > nextTrainQueueTime)
		{
			bool flag = false;
			if (!areTrainsPaused && activeTrains < CurrentSettings.MaxActiveTrainsCount)
			{
				flag = queueNewTrain();
			}
			nextTrainQueueTime = (flag ? (ElapsedTime + UnityEngine.Random.Range(CurrentSettings.TrainIntervalMin, CurrentSettings.TrainIntervalMax) * CurrentSettings.TimeScale) : (ElapsedTime + 5f));
		}
	}

	private void onTrainArrived(TrainArrivedEvent evt)
	{
		crowdManager.SpawnCrowd(evt.TrainInfo, UnityEngine.Random.Range(CrowdCount, CrowdCount + 1));
	}

	private void onTrainDeparted(TrainDepartedEvent evt)
	{
		StartCoroutine(AdjustActiveTrainsAfterDelay());
		if (evt.WasOnTime)
		{
			correctTrains++;
		}
		else
		{
			StartCoroutine(LoseHeart());
			incorrectTrains++;
		}
		if (correctTrains >= SelectedLevel<TSDifficultySettings>().TargetTrainCount)
		{
			GameEvents.Invoke(new GameFinishedEvent());
			CurrentSettings.TimeScale = 0f;
			StartCoroutine(finishAfterDelay(1f));
		}
	}

	private IEnumerator finishAfterDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Finish(succes: true);
	}

	private void onTrainPassedBy(TrainPassedByEvent evt)
	{
		StartCoroutine(LoseHeart());
		incorrectTrains++;
		StartCoroutine(AdjustActiveTrainsAfterDelay());
	}

	private bool queueNewTrain()
	{
		float num = AbsoluteElapsedTime + UnityEngine.Random.Range(CurrentSettings.TrainDepartureTimeMin, CurrentSettings.TrainDepartureTimeMax) * 60f;
		num = TS_TimeManager.GetRandomMinute(AbsoluteElapsedTime, CurrentSettings.TrainDepartureTimeMin, CurrentSettings.TrainDepartureTimeMax, CurrentSettings.timeSet);
		if (num < 0f)
		{
			return false;
		}
		activeTrains++;
		num -= num % 60f;
		TrainQueuedEvent trainQueuedEvent = new TrainQueuedEvent();
		trainQueuedEvent.QueueEnterTime = AbsoluteElapsedTime;
		trainQueuedEvent.TrainInfo = new TrainInfo
		{
			TimeMode = CurrentSettings.timeMode,
			Destination = trainDestinations.GetRandom(),
			TargetDepartureTime = num
		};
		GameEvents.Invoke(trainQueuedEvent);
		return true;
	}

	private IEnumerator LoseHeart()
	{
		hearts--;
		GetView<HeartsView>().LoseHeart();
		if (hearts <= 0)
		{
			GameEvents.Invoke(new GameFinishedEvent());
			CurrentSettings.TimeScale = 0f;
			yield return new WaitForSeconds(1f);
			Finish(succes: false);
		}
	}

	private IEnumerator AdjustActiveTrainsAfterDelay()
	{
		yield return new WaitForSeconds(3f);
		activeTrains--;
	}

	public string GetFormattedTime(float time)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(time);
		return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}";
	}
}

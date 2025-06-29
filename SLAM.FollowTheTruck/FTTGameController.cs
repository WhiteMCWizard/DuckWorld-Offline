using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTGameController : GameController
{
	[Serializable]
	public class DifficultySetting : LevelSetting
	{
		[SerializeField]
		private GameObject gameObject;

		[SerializeField]
		private float avatarRunSpeed = 10f;

		[SerializeField]
		private float distanceToTruck = 10f;

		public GameObject GameObject => gameObject;

		public float AvatarRunSpeed => avatarRunSpeed;

		public float DistanceToTruck => distanceToTruck;
	}

	private const int HEARTS_LEFT_POINTS = 50;

	[SerializeField]
	private DifficultySetting[] difficultySettings;

	[SerializeField]
	private float failFinishTime = 2f;

	[SerializeField]
	private float winFinishTime = 2f;

	[SerializeField]
	private float featherBonusPitchMin = 1f;

	[SerializeField]
	private float featherBonusPitchMax = 2f;

	private int lives = 3;

	[SerializeField]
	private float featherBonusTime = 1f;

	[SerializeField]
	private int featherBonusMax = 10;

	private int feathersPickedUp;

	private int feathersPickedUpThisBonus;

	private int featherPoints;

	public override int GameId => 7;

	public override LevelSetting[] Levels => difficultySettings;

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "CTT_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.ScroogeDuck;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTT_VICTORYWINDOW_SCORE_FEATHERS_PICKEDUP", featherPoints), featherPoints);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTT_VICTORYWINDOW_SCORE_HEARTS_LEFT", lives, 50), lives * 50);
			return dictionary;
		}
	}

	protected override void Start()
	{
		base.Start();
		for (int i = 0; i < difficultySettings.Length; i++)
		{
			difficultySettings[i].GameObject.SetActive(value: false);
		}
	}

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		for (int i = 0; i < difficultySettings.Length; i++)
		{
			difficultySettings[i].GameObject.SetActive(i == selectedLevel.Index);
		}
		FTTGameStartedEvent fTTGameStartedEvent = new FTTGameStartedEvent();
		fTTGameStartedEvent.Settings = SelectedLevel<DifficultySetting>();
		GameEvents.Invoke(fTTGameStartedEvent);
		OpenView<HeartsView>().SetTotalHeartCount(lives);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<FTTHeartLostEvent>(onHeartLost);
		GameEvents.Subscribe<FTTPickupCollectedEvent>(onPickupCollected);
		GameEvents.Subscribe<FTTGameEndedEvent>(onGameEnded);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<FTTHeartLostEvent>(onHeartLost);
		GameEvents.Unsubscribe<FTTPickupCollectedEvent>(onPickupCollected);
		GameEvents.Unsubscribe<FTTGameEndedEvent>(onGameEnded);
	}

	private void onHeartLost(FTTHeartLostEvent evt)
	{
		GetView<HeartsView>().LoseHeart();
		if (--lives <= 0)
		{
			FTTGameEndedEvent fTTGameEndedEvent = new FTTGameEndedEvent();
			fTTGameEndedEvent.Success = false;
			GameEvents.Invoke(fTTGameEndedEvent);
		}
	}

	private void onPickupCollected(FTTPickupCollectedEvent evt)
	{
		if (evt.Pickup.Type == FTTPickup.PickupType.Heart && lives < 3)
		{
			GetView<HeartsView>().FoundHeart(evt.Pickup.transform.position);
			lives++;
		}
		else if (evt.Pickup.Type == FTTPickup.PickupType.Feather)
		{
			CancelInvoke("resetFeatherBonus");
			Invoke("resetFeatherBonus", featherBonusTime);
			feathersPickedUp++;
			feathersPickedUpThisBonus++;
			featherPoints += GetFeatherBonusPoints();
			if (evt.Audio != null)
			{
				float t = (float)Mathf.Clamp(feathersPickedUpThisBonus, 0, 10) / (float)featherBonusMax;
				evt.Audio.pitch = Mathf.Lerp(featherBonusPitchMin, featherBonusPitchMax, t);
			}
		}
	}

	private void onGameEnded(FTTGameEndedEvent evt)
	{
		StartCoroutine(waitAndFinish(evt.Success, (!evt.Success) ? failFinishTime : winFinishTime));
	}

	private IEnumerator waitAndFinish(bool result, float waittime)
	{
		yield return new WaitForSeconds(waittime);
		Finish(result);
	}

	public int GetFeatherBonusPoints()
	{
		return Mathf.Clamp(feathersPickedUpThisBonus, 0, featherBonusMax);
	}

	private void resetFeatherBonus()
	{
		feathersPickedUpThisBonus = 0;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class ChaseTheBoatGame : PlatformerGame
{
	[Serializable]
	public class ChaseTheBoatDifficultySettings : LevelSetting
	{
		public int Level = 1;

		public float CompletionTime = 300f;

		public GameObject LevelRoot;
	}

	private const int POINTS_PER_HEART = 100;

	private const int POINTS_PER_FEATHER = 10;

	private const int POINTS_PER_SECOND_LEFT = 10;

	[SerializeField]
	[Header("Chase the Boat Properties")]
	private ChaseTheBoatDifficultySettings[] settings;

	private int foundFeathers;

	private Alarm timer;

	public override LevelSetting[] Levels => settings;

	public ChaseTheBoatDifficultySettings CurrentSettings => SelectedLevel<ChaseTheBoatDifficultySettings>();

	public override int GameId => 6;

	public override Dictionary<string, int> ScoreCategories
	{
		get
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_COLLECTABLES_FOUND", foundFeathers, 10), foundFeathers * 10);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_HEARTS_LEFT", hearts, 100), hearts * 100);
			dictionary.Add(StringFormatter.GetLocalizationFormatted("CTB_VICTORYWINDOW_SCORE_TIME_LEFT", Mathf.CeilToInt(timer.TimeLeft), 10), Mathf.CeilToInt(timer.TimeLeft) * 10);
			return dictionary;
		}
	}

	public override string IntroNPCKey => "NPC_NAME_DONALD";

	public override string IntroTextKey => "CTB_CINEMATICINTRO_TEXT";

	public override Portrait DuckCharacter => Portrait.DonaldDuck;

	public override void Play(LevelSetting selectedLevel)
	{
		base.Play(selectedLevel);
		for (int i = 0; i < settings.Length; i++)
		{
			settings[i].LevelRoot.SetActive(settings[i].LevelRoot == SelectedLevel<ChaseTheBoatDifficultySettings>().LevelRoot);
		}
		timer = Alarm.Create();
		timer.StartCountdown(CurrentSettings.CompletionTime, OnTimerFinished);
		OpenView<TimerView>().SetTimer(timer);
		GetView<ChaseTheBoatHUDView>().UpdateFeathers(foundFeathers);
	}

	public override void Finish(bool succes)
	{
		base.Finish(succes);
		timer.Pause();
		GetView<TimerView>().SetTimer(null);
	}

	protected void CollectFeather()
	{
		foundFeathers++;
		GetView<ChaseTheBoatHUDView>().UpdateFeathers(foundFeathers);
	}

	private IEnumerator DoFinishRoutine(P_Finish finish)
	{
		player.Pause();
		timer.Pause();
		Vector3 from = finish.entrance.transform.position;
		Vector3 to = finish.transform.position + Vector3.forward * 1f;
		player.GetComponent<Animator>().SetBool("Outro", value: true);
		player.transform.rotation = Quaternion.AngleAxis(0f, Vector3.up);
		float time = 0f;
		while (time < 1f)
		{
			time += Time.deltaTime;
			player.transform.position = Vector3.Lerp(from, to, time / 1f);
			yield return null;
		}
		bool faded = false;
		OpenView<FadeView>(delegate
		{
			faded = true;
		});
		while (!faded)
		{
			player.transform.position = to;
			yield return null;
		}
		player.GetComponent<Animator>().SetBool("Walking", value: false);
		CloseView<FadeView>();
		Finish(succes: true);
	}

	private void OnTimerFinished()
	{
		Finish(succes: false);
	}

	protected override void onPickupCollectedEvent(PickupCollectedEvent evt)
	{
		base.onPickupCollectedEvent(evt);
		if (evt.pickup is CTB_Feather)
		{
			CollectFeather();
		}
	}

	protected override void onFinishReachedEvent(FinishReachedEvent evt)
	{
		StartCoroutine(DoFinishRoutine(evt.Finish));
	}
}

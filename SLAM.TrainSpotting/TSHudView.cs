using System;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSHudView : HUDView
{
	[SerializeField]
	private GameObject hourDial;

	[SerializeField]
	private GameObject minuteDial;

	[SerializeField]
	private GameObject secondDial;

	[SerializeField]
	private UILabel lblTrainCount;

	private int correctTrainCount;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		secondDial.SetActive(Controller<TrainSpottingGame>().CurrentSettings.TimeScale <= 60f);
		updateTrainLabel();
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		if (evt.WasOnTime)
		{
			correctTrainCount++;
			updateTrainLabel();
		}
	}

	private void updateTrainLabel()
	{
		lblTrainCount.text = $"{correctTrainCount}/{Controller<TrainSpottingGame>().SelectedLevel<TrainSpottingGame.TSDifficultySettings>().TargetTrainCount}";
	}

	protected override void Update()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(Controller<TrainSpottingGame>().AbsoluteElapsedTime);
		float num = timeSpan.Hours;
		float num2 = timeSpan.Minutes;
		float num3 = timeSpan.Seconds;
		float y = num / 12f * 360f + num2 / 60f * 30f;
		Quaternion localRotation = Quaternion.Euler(0f, y, 0f);
		Quaternion quaternion = Quaternion.Euler(0f, num2 / 60f * 360f, 0f);
		if (quaternion != minuteDial.transform.localRotation)
		{
			AudioController.Play((timeSpan.Minutes % 5 != 0) ? "TS_extra_clockTick_heavy" : "TS_extra_clockTick_light");
		}
		hourDial.transform.localRotation = localRotation;
		minuteDial.transform.localRotation = quaternion;
		secondDial.transform.localRotation = Quaternion.Euler(0f, num3 / 60f * 360f, 0f);
	}
}

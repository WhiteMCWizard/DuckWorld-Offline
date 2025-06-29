using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TS_TimeManager : MonoBehaviour
{
	private static List<int> generatedNumbers = new List<int>();

	private void OnEnable()
	{
		Localization.LoadCSV(Resources.Load<TextAsset>("TimeLocalization"), merge: true);
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Subscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
	}

	private void OnDisable()
	{
		Localization.LoadCSV(Resources.Load<TextAsset>("Localization"));
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
	}

	public static float GetRandomMinute(float currentTime, float TrainDepartureTimeMin, float TrainDepartureTimeMax, TrainSpottingGame.TimeSets timeSets)
	{
		List<int> list = new List<int>();
		int num = (int)(currentTime / 60f);
		for (int num2 = generatedNumbers.Count - 1; num2 >= 0; num2--)
		{
			if (num > generatedNumbers[num2])
			{
				generatedNumbers.RemoveAt(num2);
			}
		}
		for (int i = num + (int)TrainDepartureTimeMin; i < num + (int)TrainDepartureTimeMax; i++)
		{
			if (!generatedNumbers.Contains(i))
			{
				float num3 = (float)i / 60f;
				float num4 = num3 - (float)(int)num3;
				if ((timeSets & TrainSpottingGame.TimeSets.Minutes) == TrainSpottingGame.TimeSets.Minutes)
				{
					list.Add(i);
				}
				else if (num4 == 0f && (timeSets & TrainSpottingGame.TimeSets.Hours) == TrainSpottingGame.TimeSets.Hours)
				{
					list.Add(i);
				}
				else if (num4 == 0.5f && (timeSets & TrainSpottingGame.TimeSets.Half) == TrainSpottingGame.TimeSets.Half)
				{
					list.Add(i);
				}
				else if ((num4 == 0.25f || num4 == 0.75f) && (timeSets & TrainSpottingGame.TimeSets.Quarter) == TrainSpottingGame.TimeSets.Quarter)
				{
					list.Add(i);
				}
			}
		}
		if (list.Count == 0)
		{
			return -1f;
		}
		int num5 = list[UnityEngine.Random.Range(0, list.Count)];
		generatedNumbers.Add(num5);
		return (float)num5 * 60f;
	}

	public static string GetWrittenTime(float timeInSeconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
		string key = $"{GetAnalogHour(timeSpan.Hours):00}:{timeSpan.Minutes:00}";
		return Localization.Get(key);
	}

	public static int GetAnalogHour(int hour)
	{
		if (hour > 12)
		{
			hour -= 12;
		}
		if (hour == 0)
		{
			hour = 12;
		}
		return hour;
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		generatedNumbers.Remove((int)(evt.TrainInfo.TargetDepartureTime / 60f));
	}

	private void onTrainPassedBy(TrainSpottingGame.TrainPassedByEvent evt)
	{
		generatedNumbers.Remove((int)(evt.TrainInfo.TargetDepartureTime / 60f));
	}
}

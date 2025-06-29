using System;
using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TS_TrainTimeView : View
{
	[SerializeField]
	private UILabel[] timeLabels;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
	}

	protected override void Start()
	{
		UILabel[] array = timeLabels;
		foreach (UILabel uILabel in array)
		{
			uILabel.transform.parent.gameObject.SetActive(value: false);
		}
	}

	private void onTrainArrived(TrainSpottingGame.TrainArrivedEvent evt)
	{
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
	}

	private IEnumerator UpdateTrackSign(TSTrainTrack track)
	{
		yield return new WaitForEndOfFrame();
		int trackNR = int.Parse(track.TrackName.Remove(1)) - 1;
		if (track.TrackName.Contains("a"))
		{
			trackNR += 4;
		}
		if (track.Trains.Count > 0)
		{
			timeLabels[trackNR].transform.parent.gameObject.SetActive(value: true);
			timeLabels[trackNR].transform.parent.GetComponent<TSScheduleItemView>().SetInfo(track.Trains[0].TrainObject.TrainInfo);
			TimeSpan span = TimeSpan.FromSeconds(track.Trains[0].TargetDepartureTime);
			timeLabels[trackNR].text = $"{span.Hours:00}:{span.Minutes:00}";
		}
		else
		{
			timeLabels[trackNR].transform.parent.gameObject.SetActive(value: false);
		}
	}
}

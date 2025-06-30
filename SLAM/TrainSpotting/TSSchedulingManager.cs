using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSSchedulingManager : MonoBehaviour
{
	[SerializeField]
	private TrainSpottingGame gameController;

	private TrainSpottingGame.TrainScheduleItemClickedEvent lastClickedTrainScheduleItem;

	private List<TrainSpottingGame.TrainInfo> trains = new List<TrainSpottingGame.TrainInfo>();

	private List<TrainSpottingGame.TrainInfo> pendingTrains = new List<TrainSpottingGame.TrainInfo>();

	private double lastMinuteCheck;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainTrackClickedEvent>(onTrainTrackClicked);
		GameEvents.Subscribe<TrainSpottingGame.TrainScheduleItemClickedEvent>(onTrainScheduleItemClicked);
		GameEvents.Subscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
		GameEvents.Subscribe<TrainSpottingGame.GameFinishedEvent>(onGameFinished);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainTrackClickedEvent>(onTrainTrackClicked);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainScheduleItemClickedEvent>(onTrainScheduleItemClicked);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
		GameEvents.Unsubscribe<TrainSpottingGame.GameFinishedEvent>(onGameFinished);
	}

	private void Update()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(gameController.AbsoluteElapsedTime);
		if (!(timeSpan.TotalMinutes > lastMinuteCheck))
		{
			return;
		}
		lastMinuteCheck = timeSpan.TotalMinutes;
		List<int> list = new List<int>();
		for (int num = pendingTrains.Count - 1; num >= 0; num--)
		{
			TrainSpottingGame.TrainInfo trainInfo = pendingTrains[num];
			if (TimeSpan.FromSeconds(trainInfo.TargetDepartureTime).TotalMinutes < lastMinuteCheck - 1.0 - (double)gameController.CurrentSettings.TrainDepartureThreshold)
			{
				TrainSpottingGame.TrainPassedByEvent trainPassedByEvent = new TrainSpottingGame.TrainPassedByEvent();
				trainPassedByEvent.TrainInfo = trainInfo;
				GameEvents.Invoke(trainPassedByEvent);
				pendingTrains.Remove(pendingTrains[num]);
			}
		}
		for (int i = 0; i < trains.Count; i++)
		{
			TrainSpottingGame.TrainInfo trainInfo2 = trains[i];
			if (TimeSpan.FromSeconds(trainInfo2.TargetDepartureTime).TotalMinutes < lastMinuteCheck - 1.0 - (double)gameController.CurrentSettings.TrainDepartureThreshold && trainInfo2.TrainObject.CanMoveTo(trainInfo2.TrainObject.TrainInfo.Track.EndPosition))
			{
				trainInfo2.TrainIsDeparted = true;
				TrainSpottingGame.TrainDepartedEvent trainDepartedEvent = new TrainSpottingGame.TrainDepartedEvent();
				trainDepartedEvent.DepartureTime = gameController.AbsoluteElapsedTime;
				trainDepartedEvent.TrainInfo = trainInfo2;
				trainDepartedEvent.WasOnTime = false;
				GameEvents.Invoke(trainDepartedEvent);
				list.Add(i);
			}
		}
		for (int num2 = list.Count - 1; num2 >= 0; num2--)
		{
			trains.RemoveAt(list[num2]);
		}
	}

	private void onTrainQueued(TrainSpottingGame.TrainQueuedEvent evt)
	{
		pendingTrains.Add(evt.TrainInfo);
	}

	private void onTrainScheduleItemClicked(TrainSpottingGame.TrainScheduleItemClickedEvent evt)
	{
		if (evt.ScheduleItem.CurrentTrain.Track != null)
		{
			DepartTrain(evt.ScheduleItem.CurrentTrain.Track);
			return;
		}
		if (lastClickedTrainScheduleItem != null && lastClickedTrainScheduleItem.ScheduleItem != null)
		{
			lastClickedTrainScheduleItem.ScheduleItem.UndoHighlight();
		}
		lastClickedTrainScheduleItem = evt;
	}

	private void onTrainTrackClicked(TrainSpottingGame.TrainTrackClickedEvent evt)
	{
		if (lastClickedTrainScheduleItem != null)
		{
			if (lastClickedTrainScheduleItem.ScheduleItem.CurrentTrain.Track == null)
			{
				TrainSpottingGame.TrainInfo currentTrain = lastClickedTrainScheduleItem.ScheduleItem.CurrentTrain;
				currentTrain.Track = evt.Track;
				TrainSpottingGame.TrainArrivedEvent trainArrivedEvent = new TrainSpottingGame.TrainArrivedEvent();
				trainArrivedEvent.Track = evt.Track;
				trainArrivedEvent.TrainInfo = currentTrain;
				trainArrivedEvent.ArrivalTime = gameController.AbsoluteElapsedTime;
				GameEvents.Invoke(trainArrivedEvent);
				pendingTrains.Remove(currentTrain);
				trains.Add(currentTrain);
			}
			lastClickedTrainScheduleItem = null;
		}
		else
		{
			DepartTrain(evt.Track);
		}
	}

	private void DepartTrain(TSTrainTrack track)
	{
		for (int i = 0; i < track.Trains.Count; i++)
		{
			TrainSpottingGame.TrainInfo trainInfo = track.Trains[i];
			TSTrain trainObject = trainInfo.TrainObject;
			if (trainObject.CanMoveTo(trainObject.TrainInfo.Track.EndPosition))
			{
				float num = Mathf.Abs(trainObject.TrainInfo.TargetDepartureTime - gameController.AbsoluteElapsedTime);
				trainInfo.TrainIsDeparted = true;
				TrainSpottingGame.TrainDepartedEvent trainDepartedEvent = new TrainSpottingGame.TrainDepartedEvent();
				trainDepartedEvent.DepartureTime = gameController.AbsoluteElapsedTime;
				trainDepartedEvent.TrainInfo = trainObject.TrainInfo;
				trainDepartedEvent.WasOnTime = num < gameController.CurrentSettings.TrainDepartureThreshold * 60f && trainObject.CanDepart;
				GameEvents.Invoke(trainDepartedEvent);
				trains.Remove(trainInfo);
			}
		}
	}

	private void onGameFinished(TrainSpottingGame.GameFinishedEvent evt)
	{
		UnityEngine.Object.Destroy(this);
	}
}

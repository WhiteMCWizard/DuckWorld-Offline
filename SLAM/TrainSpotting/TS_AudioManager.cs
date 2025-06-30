using UnityEngine;

namespace SLAM.TrainSpotting;

public class TS_AudioManager : MonoBehaviour
{
	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Subscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Subscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
		GameEvents.Subscribe<TrainSpottingGame.CrowdEnteredTrain>(onCrowdEnteredTrain);
		GameEvents.Subscribe<TrainSpottingGame.TrainShouldDepartEvent>(onTrainShouldDepart);
		GameEvents.Subscribe<TrainSpottingGame.TrainTrackClickedEvent>(onTrainTrackClicked);
		GameEvents.Subscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedBy);
		GameEvents.Unsubscribe<TrainSpottingGame.CrowdEnteredTrain>(onCrowdEnteredTrain);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainShouldDepartEvent>(onTrainShouldDepart);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainTrackClickedEvent>(onTrainTrackClicked);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void onTrainArrived(TrainSpottingGame.TrainArrivedEvent evt)
	{
		AudioController.Play("TS_train_arrive");
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		AudioController.Play("TS_train_depart");
		if (evt.WasOnTime)
		{
			AudioController.Play("TS_train_complete");
		}
	}

	private void onTrainPassedBy(TrainSpottingGame.TrainPassedByEvent evt)
	{
	}

	private void onCrowdEnteredTrain(TrainSpottingGame.CrowdEnteredTrain evt)
	{
		AudioController.Play("TS_passenger_whoosh");
		AudioController.Play("TS_passenger_door");
		AudioController.Play("TS_extra_bell");
	}

	private void onTrainShouldDepart(TrainSpottingGame.TrainShouldDepartEvent evt)
	{
	}

	private void onTrainTrackClicked(TrainSpottingGame.TrainTrackClickedEvent evt)
	{
		AudioController.Play("Interface_buttonClick_secundary");
	}

	private void onTrainQueued(TrainSpottingGame.TrainQueuedEvent evt)
	{
		AudioController.Play("TS_new_notice");
	}
}

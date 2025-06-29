using System.Collections;
using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TS_TutorialView : TutorialView
{
	[SerializeField]
	private ToolTip mouseClickToolTip;

	[SerializeField]
	private Vector3 scheduleOffset;

	[SerializeField]
	private Vector3 platformOffset;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueuedEvent);
		GameEvents.Subscribe<TrainSpottingGame.TrainScheduleItemClickedEvent>(onTrainScheduleItemClicked);
		GameEvents.Subscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrivedEvent);
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDepartedEvent);
		GameEvents.Subscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedByEvent);
		GameEvents.Subscribe<TrainSpottingGame.TrainShouldDepartEvent>(onTrainShouldDepart);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueuedEvent);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainScheduleItemClickedEvent>(onTrainScheduleItemClicked);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrivedEvent);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDepartedEvent);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainPassedByEvent>(onTrainPassedByEvent);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainShouldDepartEvent>(onTrainShouldDepart);
	}

	private void onTrainQueuedEvent(TrainSpottingGame.TrainQueuedEvent evt)
	{
		StartCoroutine(doTrainQueuedSequence());
	}

	private IEnumerator doTrainQueuedSequence()
	{
		mouseClickToolTip.Hide();
		yield return new WaitForSeconds(0.5f);
		mouseClickToolTip.Show(Object.FindObjectOfType<TSScheduleItemView>().transform, scheduleOffset);
		yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainScheduleItemClickedEvent>();
		mouseClickToolTip.Hide();
		yield return null;
		mouseClickToolTip.Show(Object.FindObjectOfType<TSTrainTrack>().transform, platformOffset);
		yield return CoroutineUtils.WaitForGameEvent<TrainSpottingGame.TrainArrivedEvent>();
		mouseClickToolTip.Hide();
	}

	private void onTrainScheduleItemClicked(TrainSpottingGame.TrainScheduleItemClickedEvent evt)
	{
	}

	private void onTrainArrivedEvent(TrainSpottingGame.TrainArrivedEvent evt)
	{
	}

	private void onTrainDepartedEvent(TrainSpottingGame.TrainDepartedEvent evt)
	{
	}

	private void onTrainPassedByEvent(TrainSpottingGame.TrainPassedByEvent evt)
	{
		StopAllCoroutines();
		mouseClickToolTip.Hide();
	}

	private void onTrainShouldDepart(TrainSpottingGame.TrainShouldDepartEvent evt)
	{
		mouseClickToolTip.Hide();
		mouseClickToolTip.Show(evt.scheduleItem.transform, scheduleOffset);
	}
}

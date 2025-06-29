using SLAM.Engine;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSScheduleView : View
{
	[SerializeField]
	private GameObject scheduleItemViewPrefab;

	[SerializeField]
	private UIGrid gridParent;

	private int c;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainQueuedEvent>(onTrainQueued);
	}

	private void onTrainQueued(TrainSpottingGame.TrainQueuedEvent evt)
	{
		GameObject gameObject = Object.Instantiate(scheduleItemViewPrefab);
		gameObject.GetComponentInChildren<TSScheduleItemView>().SetInfo(evt.TrainInfo);
		gameObject.name = "z" + ++c;
		gridParent.AddChild(gameObject.transform);
		gameObject.transform.localPosition = new Vector3(0f, (float)(-UICamera.currentCamera.pixelHeight) * 0.5f);
		gameObject.transform.localScale = Vector3.one;
		gridParent.Reposition();
		gridParent.repositionNow = true;
	}
}

using System.Collections;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSTrainManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] trainPrefabs;

	[SerializeField]
	private TrainSpottingGame gameController;

	[SerializeField]
	private LayerMask hitMask;

	private RaycastHit rayHit;

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

	private void Update()
	{
		if (Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, float.PositiveInfinity, hitMask.value) && rayHit.collider.transform.GetComponentInParent<TSTrainTrack>() != null)
		{
			TSTrainTrack componentInParent = rayHit.collider.transform.GetComponentInParent<TSTrainTrack>();
			if (Input.GetMouseButtonDown(0))
			{
				TrainSpottingGame.TrainTrackClickedEvent trainTrackClickedEvent = new TrainSpottingGame.TrainTrackClickedEvent();
				trainTrackClickedEvent.Track = componentInParent;
				GameEvents.Invoke(trainTrackClickedEvent);
			}
		}
	}

	private void onTrainArrived(TrainSpottingGame.TrainArrivedEvent evt)
	{
		GameObject gameObject = Object.Instantiate(trainPrefabs.GetRandom(), evt.Track.StartPosition, Quaternion.identity) as GameObject;
		evt.TrainInfo.TrainObject = gameObject.GetComponentInChildren<TSTrain>();
		evt.TrainInfo.TrainObject.SetInfo(evt.TrainInfo);
		evt.TrainInfo.TrainObject.MoveTo(evt.Track.DepartPosition);
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		StartCoroutine(doMoveToEndAndDestroy(evt));
	}

	private IEnumerator doMoveToEndAndDestroy(TrainSpottingGame.TrainDepartedEvent evt)
	{
		yield return evt.TrainInfo.TrainObject.MoveTo(evt.TrainInfo.Track.EndPosition);
		Object.Destroy(evt.TrainInfo.TrainObject.gameObject);
	}
}

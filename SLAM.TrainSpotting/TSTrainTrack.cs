using System.Collections.Generic;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSTrainTrack : MonoBehaviour
{
	[SerializeField]
	private string trackName = "1a";

	[SerializeField]
	private Vector3 startPosition = Vector3.zero;

	[SerializeField]
	private Vector3 departPosition = Vector3.zero;

	[SerializeField]
	private Vector3 endPosition = Vector3.zero;

	[SerializeField]
	private Vector3 passengerSpawnPosition = Vector3.zero;

	private List<TrainSpottingGame.TrainInfo> trains = new List<TrainSpottingGame.TrainInfo>();

	public Vector3 StartPosition => startPosition;

	public Vector3 DepartPosition => departPosition;

	public Vector3 EndPosition => endPosition;

	public Vector3 PassengerSpawnPosition => base.transform.position + passengerSpawnPosition;

	public string TrackName => trackName;

	public List<TrainSpottingGame.TrainInfo> Trains
	{
		get
		{
			return trains;
		}
		protected set
		{
			trains = value;
		}
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Subscribe<TrainSpottingGame.GameFinishedEvent>(onGameFinished);
		GameEvents.Subscribe<TrainSpottingGame.GameStartedEvent>(onGameStarted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainArrivedEvent>(onTrainArrived);
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
		GameEvents.Unsubscribe<TrainSpottingGame.GameFinishedEvent>(onGameFinished);
		GameEvents.Unsubscribe<TrainSpottingGame.GameStartedEvent>(onGameStarted);
	}

	private void Start()
	{
		onGameFinished(new TrainSpottingGame.GameFinishedEvent());
	}

	private void onTrainArrived(TrainSpottingGame.TrainArrivedEvent evt)
	{
		if (evt.TrainInfo.Track == this)
		{
			Trains.Add(evt.TrainInfo);
		}
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		if (evt.TrainInfo.Track == this)
		{
			Trains.Remove(evt.TrainInfo);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(startPosition + Vector3.up, departPosition + Vector3.up);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(departPosition + Vector3.up, endPosition + Vector3.up);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(PassengerSpawnPosition, 0.5f);
	}

	private void onGameStarted(TrainSpottingGame.GameStartedEvent evt)
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			collider.enabled = true;
		}
	}

	private void onGameFinished(TrainSpottingGame.GameFinishedEvent evt)
	{
		Collider[] componentsInChildren = GetComponentsInChildren<Collider>();
		Collider[] array = componentsInChildren;
		foreach (Collider collider in array)
		{
			collider.enabled = false;
		}
	}
}

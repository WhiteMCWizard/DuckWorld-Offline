using System.Collections;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSCrowdManager : MonoBehaviour
{
	[SerializeField]
	private GameObject[] crowdPrefabs;

	[SerializeField]
	private TSTrainTrack[] tracks;

	[SerializeField]
	private Vector3 spawnPosition = Vector3.zero;

	[SerializeField]
	private Vector3 spawnRotation = Vector3.zero;

	[SerializeField]
	private float spawnInterval;

	private void OnDrawGizmos()
	{
		GizmosUtils.DrawArrow(spawnPosition, spawnRotation);
	}

	public void SpawnCrowd(TrainSpottingGame.TrainInfo trainInfo, int count)
	{
		StartCoroutine(doSpawnCrowd(trainInfo, count));
	}

	private IEnumerator doSpawnCrowd(TrainSpottingGame.TrainInfo trainInfo, int count)
	{
		while (trainInfo.TrainObject == null || trainInfo.TrainObject.IsMoving)
		{
			yield return null;
		}
		trainInfo.TrainObject.SetPassengerTarget(count);
		for (int i = 0; i < count; i++)
		{
			if (!trainInfo.TrainIsDeparted)
			{
				GameObject go = Object.Instantiate(crowdPrefabs.GetRandom(), trainInfo.Track.PassengerSpawnPosition, Quaternion.Euler(spawnRotation)) as GameObject;
				go.transform.parent = base.transform;
				go.transform.forward = trainInfo.Track.DepartPosition - go.transform.position;
				go.GetComponentInChildren<TSCrowdAgent>().SetDestination(trainInfo);
				yield return new WaitForSeconds(spawnInterval);
			}
		}
	}
}

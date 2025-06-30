using System.Collections;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSCrowdAgent : MonoBehaviour
{
	[SerializeField]
	private NavMeshAgent navMeshAgent;

	private TSTrainTrack currentTrack;

	private TrainSpottingGame.TrainInfo currentTrainInfo;

	private void OnEnable()
	{
		GameEvents.Subscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<TrainSpottingGame.TrainDepartedEvent>(onTrainDeparted);
	}

	public void Start()
	{
		GetComponent<NavMeshAgent>().enabled = true;
	}

	public void SetDestination(TrainSpottingGame.TrainInfo trainInfo)
	{
		currentTrainInfo = trainInfo;
		currentTrack = trainInfo.Track;
		StartCoroutine(doEnterTrainRoutine());
	}

	private IEnumerator doEnterTrainRoutine()
	{
		while (currentTrainInfo.TrainObject.IsMoving)
		{
			yield return null;
		}
		Vector3 inFrontOfDept = currentTrack.DepartPosition;
		inFrontOfDept.z += base.transform.position.z;
		inFrontOfDept.z /= 2f;
		inFrontOfDept.x -= 3f;
		yield return StartCoroutine(moveToPosition(inFrontOfDept));
		yield return StartCoroutine(moveToPosition(currentTrack.DepartPosition));
		yield return new WaitForSeconds(1f * Random.value);
		currentTrainInfo.TrainObject.EnterPassenger();
		TrainSpottingGame.CrowdEnteredTrain crowdEnteredTrain = new TrainSpottingGame.CrowdEnteredTrain();
		crowdEnteredTrain.TrainInfo = currentTrainInfo;
		GameEvents.Invoke(crowdEnteredTrain);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator doReturnToExit()
	{
		yield return StartCoroutine(moveToPosition(currentTrack.PassengerSpawnPosition));
		yield return new WaitForSeconds(1f * Random.value);
		Object.Destroy(base.gameObject);
	}

	private IEnumerator moveToPosition(Vector3 targPos)
	{
		if (base.enabled)
		{
			navMeshAgent.SetDestination(targPos);
			while (navMeshAgent.pathPending)
			{
				yield return null;
			}
			while (base.enabled && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
			{
				yield return null;
			}
		}
	}

	private void onTrainDeparted(TrainSpottingGame.TrainDepartedEvent evt)
	{
		if (currentTrainInfo.TrainObject == evt.TrainInfo.TrainObject)
		{
			StopAllCoroutines();
			StartCoroutine(doReturnToExit());
		}
	}

	private Vector3 getRandomPositionInBounds(Bounds bounds)
	{
		Vector3 center = bounds.center;
		center.x += bounds.extents.x * Random.Range(-0.5f, 0.5f);
		center.z = bounds.min.z + 0.5f * Random.Range(0.5f, 1.5f);
		center.y = base.transform.position.y;
		return center;
	}
}

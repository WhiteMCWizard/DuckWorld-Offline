using System.Collections;
using UnityEngine;

namespace SLAM.TrainSpotting;

public class TSTrain : MonoBehaviour
{
	[SerializeField]
	private float movementSpeed = 20f;

	[SerializeField]
	private float stoppingDistance = 4f;

	[SerializeField]
	private AnimationCurve movementCurve;

	private int targetPassengerCount;

	public TrainSpottingGame.TrainInfo TrainInfo { get; protected set; }

	public bool IsMoving { get; protected set; }

	public bool CanDepart => targetPassengerCount <= 0 && !IsMoving;

	public void SetInfo(TrainSpottingGame.TrainInfo trainInfo)
	{
		TrainInfo = trainInfo;
	}

	public bool CanMoveTo(Vector3 targetPosition)
	{
		Vector3 vector = targetPosition - base.transform.position;
		Ray ray = new Ray(base.transform.position, vector.normalized);
		return !Physics.Raycast(ray, vector.magnitude);
	}

	public void SetPassengerTarget(int targetCount)
	{
		targetPassengerCount = targetCount;
	}

	public void EnterPassenger()
	{
		targetPassengerCount--;
	}

	public Coroutine MoveTo(Vector3 targetPosition)
	{
		StopAllCoroutines();
		return StartCoroutine(doMoveTo(targetPosition));
	}

	private IEnumerator doMoveTo(Vector3 endPosition)
	{
		IsMoving = true;
		while ((endPosition - base.transform.position).sqrMagnitude > 0.01f)
		{
			Vector3 endDir = endPosition - base.transform.position;
			Ray ray = new Ray(base.transform.position, endDir.normalized);
			Vector3 targetPosition = endPosition;
			Vector3 startPosition = base.transform.position;
			if (Physics.Raycast(ray, out var hitInfo, endDir.magnitude))
			{
				targetPosition = hitInfo.point - endDir.normalized * stoppingDistance;
			}
			Stopwatch sw = new Stopwatch(Vector3.Distance(base.transform.position, targetPosition) / movementSpeed);
			while ((bool)sw)
			{
				yield return null;
				base.transform.position = Vector3.Lerp(startPosition, targetPosition, movementCurve.Evaluate(sw.Progress));
			}
			yield return null;
		}
		IsMoving = false;
	}
}

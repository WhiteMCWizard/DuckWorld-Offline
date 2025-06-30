using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_NeighbourManager : MonoBehaviour
{
	[SerializeField]
	private AC_NeighbourWalker[] neighbours;

	[SerializeField]
	private Transform startLocation;

	[SerializeField]
	private Transform endLocation;

	[SerializeField]
	private float minTimeBetweenNeighbours = 5f;

	[SerializeField]
	private float maxTimeBetweenNeighbours = 20f;

	private List<AC_NeighbourWalker> neighboursToMeet = new List<AC_NeighbourWalker>();

	private void Start()
	{
		for (int i = 0; i < neighbours.Length; i++)
		{
			neighbours[i].SetLocations(startLocation, endLocation);
		}
		StartCoroutine(makeNeighboursWalk());
	}

	private void Update()
	{
	}

	private IEnumerator makeNeighboursWalk()
	{
		if (neighboursToMeet.Count == 0)
		{
			neighboursToMeet.AddRange(neighbours);
		}
		AC_NeighbourWalker newWalker = neighboursToMeet.GetRandom();
		neighboursToMeet.Remove(newWalker);
		newWalker.StartWalking();
		yield return new WaitForSeconds(Random.Range(minTimeBetweenNeighbours, maxTimeBetweenNeighbours));
		StartCoroutine(makeNeighboursWalk());
	}
}

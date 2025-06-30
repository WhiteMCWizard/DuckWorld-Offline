using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Hub;

public class TrafficAnimator : MonoBehaviour
{
	[SerializeField]
	private GameObject animationPathPrefab;

	[SerializeField]
	private GameObject[] uniqueVehicles;

	[SerializeField]
	private GameObject[] cars;

	[SerializeField]
	private int minCars = 5;

	[SerializeField]
	private int maxCars = 100;

	[SerializeField]
	[Range(0f, 2f)]
	private float playbackSpeed = 1f;

	private List<GameObject> trafficObjects = new List<GameObject>();

	private void OnEnable()
	{
		int num = uniqueVehicles.Length;
		int num2 = Random.Range(minCars, maxCars - num) + num;
		num2 = ((num2 >= 0) ? num2 : 0);
		for (int i = 0; i < num2; i++)
		{
			float num3 = 1f / (float)num2 / 2f;
			float offsetNormalised = (float)i / (float)num2 + Random.Range(0f - num3, num3);
			if (i < num)
			{
				spawnVehicle(offsetNormalised, uniqueVehicles);
			}
			else
			{
				spawnVehicle(offsetNormalised, cars);
			}
		}
	}

	private void OnDisable()
	{
		foreach (GameObject trafficObject in trafficObjects)
		{
			Object.Destroy(trafficObject);
		}
	}

	private void spawnVehicle(float offsetNormalised, GameObject[] vehiclePool)
	{
		GameObject gameObject = Object.Instantiate(animationPathPrefab);
		gameObject.transform.parent = base.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		GameObject gameObject2 = Object.Instantiate(vehiclePool[Random.Range(0, vehiclePool.Length)]);
		gameObject2.transform.parent = gameObject.transform.GetChild(0);
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		trafficObjects.Add(gameObject2);
		trafficObjects.Add(gameObject);
		Animation component = gameObject.GetComponent<Animation>();
		component.Stop();
		component[component.clip.name].normalizedTime = offsetNormalised;
		component[component.clip.name].speed = playbackSpeed;
		component.Play();
	}
}

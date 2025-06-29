using System;
using System.Collections;
using SLAM.Engine;
using UnityEngine;

namespace SLAM.Hub;

public class HubMarkerView : View
{
	public enum HubMarkerIcon
	{
		Location = 2,
		AvatarHouse = 0,
		Locked = 14,
		Job = 1,
		LocationGame = 2,
		Shop = 17,
		Adventure1 = 6,
		Adventure2 = 7,
		Adventure3 = 8,
		Adventure4 = 9,
		Adventure5 = 10,
		Adventure6 = 11,
		Adventure7 = 12,
		Adventure8 = 13,
		Premium = 5,
		MotionComic = 16
	}

	[SerializeField]
	protected GameObject markerPrefab;

	[SerializeField]
	protected GameObject markerRoot;

	protected GameObject spawnMarker(Vector3 position, Quaternion rotation, Vector3 scale, Material mat, HubMarkerIcon icon, bool highlighted, Action<HubMarkerButton> onClick, object data = null)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(markerPrefab, position, rotation) as GameObject;
		gameObject.transform.parent = markerRoot.transform;
		gameObject.transform.localScale = scale;
		HubMarkerButton component = gameObject.GetComponent<HubMarkerButton>();
		component.SetInfo(mat, icon, highlighted, onClick, data);
		return gameObject;
	}

	protected GameObject spawnMarkerDelayed(float delayInSeconds, Vector3 position, Quaternion rotation, Vector3 scale, Material mat, HubMarkerIcon icon, bool highlighted, Action<HubMarkerButton> onClick, object data = null)
	{
		GameObject gameObject = spawnMarker(position, rotation, scale, mat, icon, highlighted, onClick, data);
		gameObject.SetActive(value: false);
		StartCoroutine(doActivateMarkerDelayed(delayInSeconds, gameObject));
		return gameObject;
	}

	private IEnumerator doActivateMarkerDelayed(float delayInSeconds, GameObject marker)
	{
		yield return new WaitForSeconds(delayInSeconds);
		marker.SetActive(value: true);
	}

	protected void clearMarkers()
	{
		if (!(markerRoot != null))
		{
			return;
		}
		Animator[] componentsInChildren = markerRoot.GetComponentsInChildren<Animator>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject.activeInHierarchy)
			{
				componentsInChildren[i].GetComponentInParent<Collider>().enabled = false;
				StaticCoroutine.Start(waitAndShrink((float)i * 0.1f, componentsInChildren[i]));
			}
			else
			{
				UnityEngine.Object.Destroy(componentsInChildren[i].transform.parent.gameObject);
			}
		}
	}

	private IEnumerator waitAndShrink(float delay, Animator animator)
	{
		yield return new WaitForSeconds(delay);
		if (animator != null)
		{
			animator.SetBool("shrink", value: true);
		}
	}
}

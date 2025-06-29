using System.Collections;
using UnityEngine;

namespace SLAM.Sokoban;

public class SKBMarker : MonoBehaviour
{
	[SerializeField]
	private int markerType = 1;

	[SerializeField]
	private AudioClip markerCompletedClip;

	[SerializeField]
	private AudioClip markerRemovedClip;

	public bool Completed { get; protected set; }

	public int MarkerType => markerType;

	private void OnTriggerEnter(Collider col)
	{
		SKBCrate component = col.GetComponent<SKBCrate>();
		Completed = component != null && component.MarkerType == markerType;
		if (Completed)
		{
			StartCoroutine(waitForCrateToStop(component));
		}
	}

	private IEnumerator waitForCrateToStop(SKBCrate crate)
	{
		SKBAvatarController avatar = Object.FindObjectOfType<SKBAvatarController>();
		AudioController.Play(markerCompletedClip.name);
		crate.SetCompleted(complete: true);
		while (avatar.IsMoving)
		{
			yield return null;
		}
		SokobanGameController.MarkerCompletedEvent markerCompletedEvent = new SokobanGameController.MarkerCompletedEvent();
		markerCompletedEvent.Marker = this;
		GameEvents.Invoke(markerCompletedEvent);
	}

	private void OnTriggerExit(Collider col)
	{
		if (Completed)
		{
			if (col.HasComponent<SKBCrate>())
			{
				col.GetComponent<SKBCrate>().SetCompleted(complete: false);
			}
			SokobanGameController.MarkerRemovedEvent markerRemovedEvent = new SokobanGameController.MarkerRemovedEvent();
			markerRemovedEvent.Marker = this;
			GameEvents.Invoke(markerRemovedEvent);
			AudioController.Play(markerRemovedClip.name);
		}
		Completed = false;
	}
}

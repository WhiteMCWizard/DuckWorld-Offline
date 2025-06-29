using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTBreakablePier : FTTInteractable<FTTTruckController>
{
	[SerializeField]
	private string audioToPlay;

	[SerializeField]
	private GameObject objectToActivate;

	public override void OnInteract(FTTTruckController controller)
	{
		if (!string.IsNullOrEmpty(audioToPlay))
		{
			AudioController.Play(audioToPlay, base.transform.position);
		}
		if (base.transform.HasComponent<PrefabSpawner>())
		{
			GetComponent<PrefabSpawner>().SpawnAt(base.transform.position);
		}
		GetComponent<Animation>().Play();
		if (objectToActivate != null)
		{
			objectToActivate.SetActive(value: true);
		}
	}
}

using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTPickup : FTTInteractable<FTTAvatarController>
{
	public enum PickupType
	{
		Feather,
		Heart
	}

	[SerializeField]
	private PickupType type;

	[SerializeField]
	private string audioToPlay;

	public PickupType Type => type;

	public override void OnInteract(FTTAvatarController avatarController)
	{
		AudioObject audio = null;
		if (base.transform.HasComponent<PrefabSpawner>())
		{
			base.transform.GetComponent<PrefabSpawner>().SpawnAt(base.transform.position);
		}
		if (!string.IsNullOrEmpty(audioToPlay))
		{
			audio = AudioController.Play(audioToPlay);
		}
		FTTPickupCollectedEvent fTTPickupCollectedEvent = new FTTPickupCollectedEvent();
		fTTPickupCollectedEvent.Pickup = this;
		fTTPickupCollectedEvent.Audio = audio;
		GameEvents.Invoke(fTTPickupCollectedEvent);
		Object.Destroy(base.gameObject);
	}
}

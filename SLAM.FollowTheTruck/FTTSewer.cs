using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTSewer : FTTInteractable<FTTAvatarController>
{
	[Header("Sound fx")]
	[SerializeField]
	private string impactPlayerSound;

	[SerializeField]
	private Vector3 audioOffset;

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.transform.position + audioOffset, 0.5f);
	}

	public override void OnInteract(FTTAvatarController avatarController)
	{
		if (!string.IsNullOrEmpty(impactPlayerSound))
		{
			AudioController.Play(impactPlayerSound, base.transform.position + audioOffset);
		}
		avatarController.OnSewerPipeHit(this);
	}
}

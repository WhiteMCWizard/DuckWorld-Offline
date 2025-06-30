using UnityEngine;

namespace SLAM.FollowTheTruck;

public class FTTJumpToggle : FTTInteractable<FTTAvatarController>
{
	[SerializeField]
	private bool allowJump;

	public override void OnInteract(FTTAvatarController controller)
	{
		controller.SetJumpAllowed(allowJump);
	}
}

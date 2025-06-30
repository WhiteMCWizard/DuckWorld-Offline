using UnityEngine;

namespace SLAM.Avatar;

[RequireComponent(typeof(Animator))]
public class AvatarGenderAnimations : MonoBehaviour
{
	[SerializeField]
	private AnimatorOverrideController boyController;

	[SerializeField]
	private AnimatorOverrideController girlController;

	private void Start()
	{
		Animator component = GetComponent<Animator>();
		AvatarConfigurationData playerConfiguration = AvatarSystem.GetPlayerConfiguration();
		if (playerConfiguration.Gender == AvatarSystem.Gender.Boy && boyController != null)
		{
			component.runtimeAnimatorController = boyController;
		}
		else if (playerConfiguration.Gender == AvatarSystem.Gender.Girl && girlController != null)
		{
			component.runtimeAnimatorController = girlController;
		}
	}
}

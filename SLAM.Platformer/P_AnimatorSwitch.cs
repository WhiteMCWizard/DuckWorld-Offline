using UnityEngine;

namespace SLAM.Platformer;

public class P_AnimatorSwitch : P_Toggle
{
	[SerializeField]
	private Animator[] animators;

	[SerializeField]
	private string activatedTrigger = "activate";

	[SerializeField]
	private string deactivatedTrigger = "deactivate";

	protected override void OnTogglePressed(bool state)
	{
		Animator[] array = animators;
		foreach (Animator animator in array)
		{
			animator.SetTrigger((!state) ? deactivatedTrigger : activatedTrigger);
		}
	}
}

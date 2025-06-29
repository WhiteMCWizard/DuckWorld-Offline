using UnityEngine;

public class MB_FlintheartIK : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Transform lookIK;

	[SerializeField]
	private Transform leftHandIK;

	[SerializeField]
	private Transform rightHandIK;

	[Range(0f, 1f)]
	[SerializeField]
	private float ikLeftRange = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float ikRightRange = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float ikLookRange = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float bodyWeight = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float headWeight = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float eyesWeight = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float clampWeight = 1f;

	private void OnAnimatorIK(int l)
	{
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftRange);
		animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikLeftRange);
		animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIK.position);
		animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIK.rotation);
		animator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikRightRange);
		animator.SetIKRotationWeight(AvatarIKGoal.RightHand, ikRightRange);
		animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIK.position);
		animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIK.rotation);
		animator.SetLookAtPosition(lookIK.position);
		animator.SetLookAtWeight(ikLookRange, bodyWeight, headWeight, eyesWeight, clampWeight);
	}

	public void SetIKWeight(AvatarIKGoal goal, float weight)
	{
		switch (goal)
		{
		case AvatarIKGoal.RightHand:
			ikRightRange = weight;
			break;
		case AvatarIKGoal.LeftHand:
			ikLeftRange = weight;
			break;
		}
	}
}

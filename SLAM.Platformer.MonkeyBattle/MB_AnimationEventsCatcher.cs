using UnityEngine;

namespace SLAM.Platformer.MonkeyBattle;

public class MB_AnimationEventsCatcher : MonoBehaviour
{
	[SerializeField]
	private string footstep;

	[SerializeField]
	private string footstep_crouched;

	[SerializeField]
	private string footstep_ladder;

	[SerializeField]
	private string lever;

	[SerializeField]
	private string land;

	[SerializeField]
	private string doubleJump;

	[SerializeField]
	private string jump;

	private MB_PlayerController avatar;

	private void Start()
	{
		avatar = GetComponent<MB_PlayerController>();
	}

	private void Update()
	{
	}

	private void OnLeverPush()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(lever);
		}
	}

	private void OnFootstep()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist() && !avatar.IsJumping)
		{
			AudioController.Play(footstep);
		}
	}

	private void OnFootstepCrouched()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(footstep_crouched);
		}
	}

	private void OnLand()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(land);
		}
	}

	private void OnClimbLadder()
	{
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
		{
			AudioController.Play(footstep_ladder);
		}
	}

	private void OnJump()
	{
	}

	private void OnDoubleJump()
	{
	}
}

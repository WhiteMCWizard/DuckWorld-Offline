using UnityEngine;

public class CTB_AnimationEventsCatcher : MonoBehaviour
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

	private void Start()
	{
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
		if ((bool)SingletonMonoBehaviour<AudioController>.DoesInstanceExist())
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

using System.Collections;
using UnityEngine;

namespace SLAM.Platformer;

public abstract class P_Toggle : P_PressUpTrigger
{
	[SerializeField]
	protected float toggleDelay;

	[SerializeField]
	protected float lockAvatarDuration = 1.4f;

	[SerializeField]
	protected bool isToggled;

	[SerializeField]
	protected bool manualToggle;

	[SerializeField]
	protected bool oneTimeToggle;

	[SerializeField]
	protected bool useInterval;

	[SerializeField]
	protected float interval = 2f;

	protected Animator animator;

	public bool IsToggled => isToggled;

	protected override void Start()
	{
		base.Start();
		animator = GetComponent<Animator>();
		OnTogglePressed(isToggled);
		if (!manualToggle && useInterval)
		{
			StartCoroutine(intervalRoutine());
		}
		PlayAnimation(0);
	}

	protected override UpAction DoAction()
	{
		if (manualToggle)
		{
			isToggled = !isToggled;
			if (toggleDelay > 0.001f)
			{
				StartCoroutine(DelayToggle(toggleDelay, isToggled));
			}
			else
			{
				OnTogglePressed(isToggled);
			}
			if (useInterval)
			{
				manualToggle = false;
				StartCoroutine(intervalRoutine());
			}
			manualToggle = !oneTimeToggle && manualToggle;
			return new UpAction(Action.PressSwitch, lockAvatarDuration, this);
		}
		return base.DoAction();
	}

	private IEnumerator DelayToggle(float duration, bool state)
	{
		yield return new WaitForSeconds(duration);
		OnTogglePressed(state);
	}

	protected abstract void OnTogglePressed(bool state);

	protected IEnumerator intervalRoutine()
	{
		yield return new WaitForSeconds(interval + toggleDelay);
		toggleDelay = 0f;
		isToggled = !isToggled;
		OnTogglePressed(isToggled);
		StartCoroutine(intervalRoutine());
	}

	public void PlayAnimation(int avatarDirection)
	{
		animator.SetInteger("AvatarDirection", avatarDirection);
		animator.SetInteger("HandleState", isToggled ? 1 : 0);
	}
}

using System;
using UnityEngine;

namespace SLAM.Platformer;

public class P_PressUpTrigger : P_Trigger
{
	private CC2DPlayer _player;

	private bool isInZone;

	protected CC2DPlayer Player
	{
		get
		{
			if (_player == null)
			{
				_player = UnityEngine.Object.FindObjectOfType<CC2DPlayer>();
			}
			return _player;
		}
	}

	public bool IsInZone => isInZone;

	private UpAction OnActionPressed()
	{
		if (isInZone)
		{
			return DoAction();
		}
		return UpAction.NoAction;
	}

	protected override void OnTriggerEnter(Collider other)
	{
		CC2DPlayer componentInParent = other.GetComponentInParent<CC2DPlayer>();
		if ((bool)componentInParent)
		{
			base.OnTriggerEnter(other);
			isInZone = true;
			componentInParent.OnActionKeyPressed = (CC2DPlayer.ActionKeyPressed)Delegate.Combine(componentInParent.OnActionKeyPressed, new CC2DPlayer.ActionKeyPressed(OnActionPressed));
		}
	}

	protected override void OnTriggerExit(Collider other)
	{
		CC2DPlayer componentInParent = other.GetComponentInParent<CC2DPlayer>();
		if ((bool)componentInParent)
		{
			base.OnTriggerExit(other);
			isInZone = false;
			componentInParent.OnActionKeyPressed = (CC2DPlayer.ActionKeyPressed)Delegate.Remove(componentInParent.OnActionKeyPressed, new CC2DPlayer.ActionKeyPressed(OnActionPressed));
		}
	}

	protected virtual UpAction DoAction()
	{
		return UpAction.NoAction;
	}
}

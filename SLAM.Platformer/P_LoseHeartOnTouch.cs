using System;
using System.Collections;
using UnityEngine;

namespace SLAM.Platformer;

public class P_LoseHeartOnTouch : P_Trigger
{
	[SerializeField]
	private float cooldownDuration = 1f;

	[SerializeField]
	private AudioClip audioClip;

	private bool cooldownActive;

	private void OnTriggerStay(Collider col)
	{
		if (!cooldownActive && col.transform.root.GetComponentInChildren<CC2DPlayer>() != null)
		{
			StartCoroutine(waitTillEndOfFrameAndExecute(delegate
			{
				GameEvents.Invoke(new PlayerHitEvent
				{
					EnemyObject = base.gameObject
				});
			}));
			StartCoroutine(cooldown(cooldownDuration));
		}
	}

	protected IEnumerator waitTillEndOfFrameAndExecute(System.Action method)
	{
		yield return new WaitForEndOfFrame();
		if (audioClip != null)
		{
			AudioController.Play(audioClip.name, base.transform);
		}
		method?.Invoke();
	}

	protected IEnumerator cooldown(float duration)
	{
		cooldownActive = true;
		yield return new WaitForSeconds(duration);
		cooldownActive = false;
	}
}

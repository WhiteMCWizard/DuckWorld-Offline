using System;
using System.Collections;
using SLAM.Engine;
using SLAM.Platformer;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_MovingEnemy : ObjectMover
{
	[SerializeField]
	protected Animator animator;

	[SerializeField]
	protected float cooldownDuration = 1f;

	[SerializeField]
	protected AudioClip movingSound;

	[SerializeField]
	protected AudioClip attackSound;

	[SerializeField]
	protected AudioClip squeakSound;

	protected bool cooldownActive;

	protected bool isAttacking;

	protected AudioObject movingAudioObject;

	protected virtual float timeBetweenAttackAnimAndHitEvent => 0.15f;

	private void Start()
	{
		if (movingSound != null)
		{
			movingAudioObject = AudioController.Play(movingSound.name, base.transform);
		}
		if (squeakSound != null)
		{
			AudioController.Play(squeakSound.name, base.transform);
		}
	}

	private void Update()
	{
		if (movingSound != null && (movingAudioObject == null || !movingAudioObject.IsPlaying()))
		{
			movingAudioObject = AudioController.Play(movingSound.name, base.transform);
		}
	}

	protected virtual IEnumerator OnTriggerEnter(Collider col)
	{
		bool isPlayer = col.GetComponentInParent<CC2DPlayer>() != null;
		if (!isAttacking)
		{
			if (movingAudioObject != null && movingAudioObject.IsPlaying())
			{
				movingAudioObject.Stop();
			}
			if (attackSound != null)
			{
				AudioController.Play(attackSound.name, base.transform);
			}
			isAttacking = true;
			animator.SetTrigger("Attack");
			float savedSpeed = movementSpeed;
			movementSpeed = 0f;
			yield return new WaitForSeconds(timeBetweenAttackAnimAndHitEvent);
			if (!cooldownActive && isPlayer)
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
			yield return new WaitForSeconds(0.5f);
			if (movingSound != null)
			{
				movingAudioObject = AudioController.Play(movingSound.name, base.transform);
			}
			movementSpeed = savedSpeed;
			TurnAround();
			isAttacking = false;
		}
		yield return null;
	}

	protected IEnumerator waitTillEndOfFrameAndExecute(System.Action method)
	{
		yield return new WaitForEndOfFrame();
		method?.Invoke();
	}

	protected IEnumerator cooldown(float duration)
	{
		cooldownActive = true;
		yield return new WaitForSeconds(duration);
		cooldownActive = false;
	}
}

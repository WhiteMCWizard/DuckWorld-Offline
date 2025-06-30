using System.Collections;
using UnityEngine;

namespace SLAM.MoneyDive;

public class MD_AvatarController : MonoBehaviour
{
	public const float FALLSPEED = 15f;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private MD_Controller gameController;

	private int comboCounter;

	private bool paused;

	private bool isFalling;

	private Trick[] tricksPool;

	private Trick currentTrick;

	private float trickFailedDuration = 1.2f;

	public bool AreControlsLocked { get; set; }

	private void Update()
	{
		if (isFalling)
		{
			base.transform.position += Vector3.down * 15f * Time.deltaTime;
			if (!paused && !AreControlsLocked && currentTrick == null)
			{
				currentTrick = GetNextTrick(tricksPool);
			}
		}
	}

	public void Init(Trick[] allowedTricks)
	{
		tricksPool = allowedTricks;
	}

	public void StartFalling()
	{
		isFalling = true;
	}

	public void StopFalling()
	{
		isFalling = false;
	}

	public void Pause()
	{
		paused = true;
	}

	public void UnPause()
	{
		paused = false;
	}

	public void DoGoodTrick()
	{
		StartCoroutine(LockDuringAnimation(currentTrick.Duration));
		animator.SetTrigger(currentTrick.Name);
		AudioController.Play("MD_arrowkey_correct");
		comboCounter++;
		gameController.TrickPerformed(succes: true, currentTrick, comboCounter);
		currentTrick = GetNextTrick(tricksPool);
	}

	public void DoFailTrick()
	{
		StartCoroutine(LockDuringAnimation(trickFailedDuration));
		animator.SetTrigger("Interupt");
		AudioController.Play("MD_arrowkey_incorrect");
		comboCounter = 0;
		gameController.TrickPerformed(succes: false, currentTrick, comboCounter);
		currentTrick = GetNextTrick(tricksPool);
	}

	private Trick GetNextTrick(Trick[] fromTrick)
	{
		int num = comboCounter % fromTrick.Length;
		return fromTrick[num];
	}

	private IEnumerator LockDuringAnimation(float time)
	{
		AreControlsLocked = true;
		yield return new WaitForSeconds(time);
		AreControlsLocked = false;
	}
}

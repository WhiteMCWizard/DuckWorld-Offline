using System.Collections;
using UnityEngine;

namespace SLAM.CraneOperator;

public class COScroogeAnimator : MonoBehaviour
{
	[SerializeField]
	private float minDelay;

	[SerializeField]
	private float maxDelay;

	private Animator animator;

	private float animationSpeed;

	private bool isAnimating;

	private void OnEnable()
	{
		GameEvents.Subscribe<CraneOperatorGame.GameStartedEvent>(onGameStarted);
		GameEvents.Subscribe<CraneOperatorGame.TruckCompletedEvent>(onTruckCompleted);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<CraneOperatorGame.GameStartedEvent>(onGameStarted);
		GameEvents.Unsubscribe<CraneOperatorGame.TruckCompletedEvent>(onTruckCompleted);
	}

	private void Start()
	{
		animator = GetComponent<Animator>();
		animationSpeed = animator.speed;
	}

	private void onGameStarted(CraneOperatorGame.GameStartedEvent evt)
	{
		StartCoroutine(doScroogeAngryAnimation(0f));
	}

	private void onTruckCompleted(CraneOperatorGame.TruckCompletedEvent evt)
	{
		if (!isAnimating)
		{
			StopAllCoroutines();
			animator.speed = animationSpeed;
			animator.SetTrigger("DoHappy");
		}
	}

	private IEnumerator doScroogeAngryAnimation(float delay)
	{
		animator.speed = 0f;
		yield return new WaitForSeconds(delay);
		if (!isAnimating)
		{
			isAnimating = true;
			animator.speed = animationSpeed;
			animator.SetTrigger("DoAngry");
		}
	}

	public void OnScroogeAnimationEnded()
	{
		isAnimating = false;
		StartCoroutine(doScroogeAngryAnimation(Random.Range(minDelay, maxDelay)));
	}
}

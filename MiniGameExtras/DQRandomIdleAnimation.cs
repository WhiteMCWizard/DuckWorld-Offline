using UnityEngine;

public class DQRandomIdleAnimation : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		animator.Play("Idle", 0, Random.value);
	}

	private void Update()
	{
	}
}

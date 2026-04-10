using UnityEngine;

public class SpawnOneTimeAndSetAnimator : SpawnOneTime
{
	[SerializeField]
	private RuntimeAnimatorController animController;

	protected override void Start()
	{
		base.Start();
		go.GetComponentInChildren<Animator>().runtimeAnimatorController = animController;
	}
}

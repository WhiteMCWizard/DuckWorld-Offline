using UnityEngine;

namespace SLAM.BatCave;

public class BC_HangingBat : MonoBehaviour
{
	[SerializeField]
	private Animator animator;

	[SerializeField]
	private bool randomRotation;

	private void Start()
	{
		if (randomRotation)
		{
			base.transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
		}
		animator.Play("BC_Bat_Idle", 0, Random.Range(0f, 1f));
	}

	private void Update()
	{
	}
}

using UnityEngine;

namespace SLAM.Sokoban;

public class SKBLevelExit : MonoBehaviour
{
	[SerializeField]
	private AudioClip openClip;

	[SerializeField]
	private AudioClip closeClip;

	[SerializeField]
	private Animator animator;

	public void OpenDoors()
	{
		GetComponent<Collider>().isTrigger = true;
		animator.SetTrigger("OpenDoors");
		AudioController.Play(openClip.name);
	}

	public void CloseDoors()
	{
		if (GetComponent<Collider>().isTrigger)
		{
			animator.SetTrigger("CloseDoors");
			GetComponent<Collider>().isTrigger = false;
			AudioController.Play(closeClip.name);
		}
	}
}

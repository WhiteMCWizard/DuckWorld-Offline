using UnityEngine;

public class Blender : MonoBehaviour
{
	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		animator.SetBool("1", Input.GetKeyDown(KeyCode.Alpha1));
		animator.SetBool("2", Input.GetKeyDown(KeyCode.Alpha2));
		animator.SetBool("3", Input.GetKeyDown(KeyCode.Alpha3));
		animator.SetBool("4", Input.GetKeyDown(KeyCode.Alpha4));
		animator.SetBool("5", Input.GetKeyDown(KeyCode.Alpha5));
		animator.SetBool("6", Input.GetKeyDown(KeyCode.Alpha6));
		animator.SetBool("7", Input.GetKeyDown(KeyCode.Alpha7));
		animator.SetBool("8", Input.GetKeyDown(KeyCode.Alpha8));
		animator.SetBool("9", Input.GetKeyDown(KeyCode.Alpha9));
	}
}

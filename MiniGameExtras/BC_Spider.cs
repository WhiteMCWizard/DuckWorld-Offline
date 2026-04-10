using UnityEngine;

public class BC_Spider : MonoBehaviour
{
	[SerializeField]
	[Range(0f, 1f)]
	private float startAtNormalised;

	[SerializeField]
	private bool startAtRandomTime = true;

	private void OnEnable()
	{
		float num = 0f;
		num = ((!startAtRandomTime) ? startAtNormalised : Random.Range(0f, 1f));
		GetComponent<Animator>().Play("BC_Spider_Up_Down", 0, num);
	}
}

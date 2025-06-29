using System.Collections;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_Visitor : MonoBehaviour
{
	[Range(0.1f, 10f)]
	[SerializeField]
	private float normalizedAnimationSpeed = 1f;

	[Range(0.1f, 10f)]
	[SerializeField]
	private float movementSpeed = 1.4f;

	private Vector3 startPosition;

	private Vector3 endPosition;

	protected virtual void Start()
	{
		base.gameObject.SetActive(value: false);
	}

	public void WalkFromAToB(Vector3 A, Vector3 B)
	{
		base.gameObject.SetActive(value: true);
		startPosition = A;
		endPosition = B;
		StartCoroutine(doVisit());
	}

	private IEnumerator doVisit()
	{
		GetComponent<Animator>().speed = normalizedAnimationSpeed;
		base.transform.forward = (endPosition - startPosition).normalized;
		float distance = Vector3.Distance(startPosition, endPosition);
		float duration = distance / movementSpeed;
		float time = 0f;
		while (time < duration)
		{
			time += Time.deltaTime;
			base.transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
			yield return null;
		}
		base.gameObject.SetActive(value: false);
	}

	private void OnFootstep()
	{
	}
}

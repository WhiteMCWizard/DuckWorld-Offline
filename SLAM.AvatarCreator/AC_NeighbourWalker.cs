using System.Collections;
using UnityEngine;

namespace SLAM.AvatarCreator;

public class AC_NeighbourWalker : MonoBehaviour
{
	[SerializeField]
	protected float minMovementSpeed = 0.75f;

	[SerializeField]
	protected float maxMovementSpeed = 1.6f;

	[SerializeField]
	protected float animationWalkSpeed = 1f;

	protected Transform startLocation;

	protected Transform endLocation;

	protected virtual void Start()
	{
		base.gameObject.SetActive(value: false);
	}

	public virtual void SetLocations(Transform begin, Transform end)
	{
		startLocation = begin;
		endLocation = end;
	}

	public virtual void StartWalking()
	{
		base.gameObject.SetActive(value: true);
		Vector3 position;
		Vector3 position2;
		if (Random.Range(0, 2) == 0)
		{
			position = startLocation.position;
			position2 = endLocation.position;
		}
		else
		{
			position = endLocation.position;
			position2 = startLocation.position;
		}
		StartCoroutine(doWalkPastWindow(position, position2, Random.Range(minMovementSpeed, maxMovementSpeed)));
	}

	protected virtual IEnumerator doWalkPastWindow(Vector3 from, Vector3 to, float speed)
	{
		GetComponent<Animator>().speed = speed / animationWalkSpeed;
		base.transform.forward = (to - from).normalized;
		float distance = Vector3.Distance(from, to);
		float duration = distance / speed;
		float time = 0f;
		while (time < duration)
		{
			time += Time.deltaTime;
			base.transform.position = Vector3.Lerp(from, to, time / duration);
			yield return null;
		}
		base.gameObject.SetActive(value: false);
	}

	private void OnFootstep()
	{
	}
}

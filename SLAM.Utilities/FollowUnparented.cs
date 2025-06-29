using UnityEngine;

namespace SLAM.Utilities;

public class FollowUnparented : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	[SerializeField]
	private bool includeRotation;

	[SerializeField]
	private Vector3 offset;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.position = target.position + offset;
		if (includeRotation)
		{
			base.transform.rotation = target.rotation;
		}
	}
}

using UnityEngine;

namespace SLAM.FollowTheTruck;

[ExecuteInEditMode]
public class FTTCameraController : MonoBehaviour
{
	[SerializeField]
	private Vector3 offset;

	[SerializeField]
	private Transform followObject;

	[SerializeField]
	private float damping = 1f;

	private void LateUpdate()
	{
		if (followObject != null)
		{
			Vector3 position = base.transform.position;
			if (Application.isPlaying)
			{
				position.z = Mathf.Lerp(base.transform.position.z, followObject.position.z - offset.z, damping * Time.deltaTime);
			}
			else
			{
				position.z = followObject.position.z - offset.z;
			}
			base.transform.position = position;
		}
	}
}

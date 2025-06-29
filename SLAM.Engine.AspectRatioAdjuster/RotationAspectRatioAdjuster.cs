using UnityEngine;

namespace SLAM.Engine.AspectRatioAdjuster;

public class RotationAspectRatioAdjuster : AspectRatioAdjuster
{
	[HideInInspector]
	[SerializeField]
	private Quaternion targetRotation;

	private Quaternion originalRotation;

	private void Awake()
	{
		originalRotation = base.transform.rotation;
	}

	protected override void Adjust(float aspectRatioFactor)
	{
		base.transform.rotation = Quaternion.Lerp(targetRotation, originalRotation, aspectRatioFactor);
	}

	[ContextMenu("Set current rotation to target")]
	private void setTarget()
	{
		targetRotation = base.transform.rotation;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(base.transform.position, base.transform.position + base.transform.forward);
		Gizmos.color = Color.green;
		Gizmos.DrawLine(base.transform.position, base.transform.position + targetRotation * Vector3.forward);
	}
}

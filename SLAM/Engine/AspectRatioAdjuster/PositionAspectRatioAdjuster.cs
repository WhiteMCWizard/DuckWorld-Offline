using UnityEngine;

namespace SLAM.Engine.AspectRatioAdjuster;

public class PositionAspectRatioAdjuster : AspectRatioAdjuster
{
	[SerializeField]
	[HideInInspector]
	private Vector3 targetPosition;

	private Vector3 originalPosition;

	private void Awake()
	{
		originalPosition = base.transform.position;
	}

	protected override void Adjust(float aspectRatioFactor)
	{
		base.transform.position = Vector3.Lerp(targetPosition, originalPosition, aspectRatioFactor);
	}

	[ContextMenu("Set current position to target")]
	private void setTarget()
	{
		targetPosition = base.transform.position;
	}

	private void OnDrawGizmosSelected()
	{
		Vector3 vector = ((!Application.isPlaying) ? base.transform.position : originalPosition);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(vector, targetPosition);
	}
}

using UnityEngine;

namespace SLAM.Engine.AspectRatioAdjuster;

public class FovAspectRatioAdjuster : AspectRatioAdjuster
{
	[SerializeField]
	private Camera targetCamera;

	[SerializeField]
	private float targetFov;

	private float originalFov;

	private void Awake()
	{
		originalFov = targetCamera.fieldOfView;
	}

	private void OnValidate()
	{
		if (targetCamera == null)
		{
			targetCamera = GetComponent<Camera>();
		}
	}

	private void Reset()
	{
		if (targetCamera == null)
		{
			targetCamera = GetComponent<Camera>();
		}
	}

	protected override void Adjust(float aspectRatioFactor)
	{
		targetCamera.fieldOfView = Mathf.Lerp(targetFov, originalFov, aspectRatioFactor);
	}

	private void OnDrawGizmosSelected()
	{
		if (!(targetCamera == null))
		{
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.color = Color.yellow;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawFrustum(Vector3.zero, targetFov, targetCamera.farClipPlane, targetCamera.nearClipPlane, targetCamera.aspect);
			Gizmos.matrix = matrix;
		}
	}
}

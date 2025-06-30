using UnityEngine;

namespace SLAM.CameraSystem;

public abstract class TargetObjectBehaviour : CameraBehaviour
{
	[SerializeField]
	protected Transform target;

	[HideInInspector]
	public Vector3 position;

	public void SetTarget(Transform target)
	{
		this.target = target;
	}

	public void CachePosition()
	{
		position = base.transform.position;
	}
}

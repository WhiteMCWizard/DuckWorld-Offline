using UnityEngine;

namespace SLAM.CameraSystem;

public abstract class CameraBehaviour : MonoBehaviour
{
	public float Weight;

	[SerializeField]
	private bool activateOnEnable = true;

	private Transform localTransform;

	public virtual int Layer => 1;

	public CameraManager CameraManager { get; private set; }

	protected Transform LocalTransform
	{
		get
		{
			if (localTransform == null)
			{
				localTransform = base.transform;
			}
			return localTransform;
		}
	}

	protected virtual void Start()
	{
		if (activateOnEnable)
		{
			CameraManager.CrossFade(this, 0f);
		}
	}

	protected virtual void OnEnable()
	{
		CameraManager = GetComponentInParent<CameraManager>();
		if (CameraManager != null)
		{
			CameraManager.AddBehaviour(this);
		}
		else
		{
			Debug.LogWarning("Couldnt find parent CameraSystem! Please attach it to a system", base.gameObject);
		}
	}

	protected virtual void OnDisable()
	{
		if (CameraManager != null)
		{
			CameraManager.RemoveBehaviour(this);
		}
	}

	public abstract void GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

	public Coroutine CrossFadeIn(float duration = 0f)
	{
		return CameraManager.CrossFade(this, duration);
	}
}

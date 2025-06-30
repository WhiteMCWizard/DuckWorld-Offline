using UnityEngine;

namespace SLAM.Platformer;

public class P_Bridge : MonoBehaviour
{
	[SerializeField]
	private float openAngle = 45f;

	[SerializeField]
	private float closedAngle;

	[SerializeField]
	private AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	private float time;

	private float sign = -1f;

	private float duration = 2f;

	private float angle;

	private void Start()
	{
	}

	private void Update()
	{
		time += Time.deltaTime * sign;
		angle = Mathf.Lerp(openAngle, closedAngle, curve.Evaluate(time / duration));
		base.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void Toggle(bool state)
	{
		if (state)
		{
			time = Mathf.Clamp(time, 0f, duration - time);
			AudioController.Play("CTB_bridge_lower_or_raise", base.transform);
		}
		else
		{
			time = Mathf.Clamp(time, 0f, Mathf.Min(time, duration));
			AudioController.Play("CTB_bridge_lower_or_raise", base.transform);
		}
		sign = (state ? 1 : (-1));
	}
}

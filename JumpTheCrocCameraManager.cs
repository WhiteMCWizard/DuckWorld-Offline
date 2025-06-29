using System.Collections;
using UnityEngine;

public class JumpTheCrocCameraManager : MonoBehaviour
{
	[SerializeField]
	private AnimationCurve heightCurve;

	[SerializeField]
	private AnimationCurve moveCurve;

	[SerializeField]
	private float flyDuration;

	[SerializeField]
	private Vector3 offsetToTarget;

	private Vector3 target;

	public void FlyTo(Vector3 position)
	{
		target = position + offsetToTarget;
		target.x = 0f;
		StartCoroutine(doJump(base.transform.position, target));
	}

	private IEnumerator doJump(Vector3 from, Vector3 to)
	{
		float time = 0f;
		while (time < flyDuration)
		{
			time += Time.deltaTime / flyDuration;
			float norm = time / flyDuration;
			float y = LerpOverflow(from.y, to.y, heightCurve.Evaluate(norm));
			float z = LerpOverflow(from.z, to.z, moveCurve.Evaluate(norm));
			base.transform.position = new Vector3(0f, y, z);
			yield return null;
		}
	}

	private float LerpOverflow(float from, float to, float time)
	{
		return from + (to - from) * time;
	}
}

using System.Collections;
using UnityEngine;

namespace SLAM.Engine;

public class ObjectMover : MonoBehaviour
{
	public Vector3 endPosition = Vector3.one;

	[SerializeField]
	protected AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	[SerializeField]
	protected float movementSpeed = 1f;

	[SerializeField]
	protected bool pingPong;

	protected Vector3 startPosition;

	protected float t;

	public float MovementSpeed => movementSpeed;

	protected virtual void OnDrawGizmos()
	{
		if (!Application.isPlaying)
		{
			Vector3 position = base.transform.position;
			Vector3 vector = position;
			for (float num = 0f; num < 1f; num += 0.01f)
			{
				Vector3 vector2 = Vector3.Lerp(position, endPosition + base.transform.position, num);
				vector2.y = MathUtilities.LerpUnclamped(position.y, endPosition.y + position.y, curve.Evaluate(num));
				Gizmos.DrawLine(vector, vector2);
				vector = vector2;
			}
		}
	}

	protected virtual void OnEnable()
	{
		t = 0f;
		startPosition = base.transform.position;
		Move();
	}

	public virtual void TurnAround()
	{
		movementSpeed *= -1f;
	}

	public virtual void Move()
	{
		StartCoroutine(HandleMovement());
	}

	protected virtual IEnumerator HandleMovement()
	{
		while (true)
		{
			yield return null;
			t = HandleTime(t);
			Vector3 pos = Vector3.Lerp(startPosition, endPosition + startPosition, t);
			pos.y = MathUtilities.LerpUnclamped(startPosition.y, endPosition.y + startPosition.y, curve.Evaluate(t));
			base.transform.position = pos;
		}
	}

	protected float HandleTime(float t)
	{
		t += Time.deltaTime * movementSpeed;
		if (!pingPong)
		{
			if (t > 1f)
			{
				t -= 1f;
			}
		}
		else if (t > 1f)
		{
			TurnAround();
			t = 2f - t;
		}
		else if (t < 0f)
		{
			TurnAround();
			t *= -1f;
		}
		return t;
	}
}

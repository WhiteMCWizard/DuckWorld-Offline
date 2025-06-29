using UnityEngine;

namespace SLAM;

public static class MathUtilities
{
	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
	{
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * 57.29578f;
	}

	public static float LerpUnclamped(float a, float b, float t)
	{
		return a + t * (b - a);
	}

	public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
	{
		return new Vector3(LerpUnclamped(a.x, b.x, t), LerpUnclamped(a.y, b.y, t), LerpUnclamped(a.z, b.z, t));
	}

	public static Vector3 GetClosetPoint(Vector3 A, Vector3 B, Vector3 P, bool segmentClamp)
	{
		Vector3 vector = P - A;
		Vector3 vector2 = B - A;
		float num = vector2.x * vector2.x + vector2.y * vector2.y;
		float num2 = vector.x * vector2.x + vector.y * vector2.y;
		float num3 = num2 / num;
		if (segmentClamp)
		{
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			else if (num3 > 1f)
			{
				num3 = 1f;
			}
		}
		return A + vector2 * num3;
	}

	public static bool AreDirectionsSimilar(Vector3 dirA, Vector3 dirB)
	{
		return Mathf.Approximately(Vector3.Dot(dirA, dirB), 1f);
	}
}

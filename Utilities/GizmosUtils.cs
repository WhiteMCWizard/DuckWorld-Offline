using UnityEngine;

public static class GizmosUtils
{
	public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		Gizmos.DrawRay(pos, direction);
		if (!(direction.sqrMagnitude < 0.5f))
		{
			Vector3 vector = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f + arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
			Vector3 vector2 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f - arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
			Gizmos.DrawRay(pos + direction, vector * arrowHeadLength);
			Gizmos.DrawRay(pos + direction, vector2 * arrowHeadLength);
		}
	}
}

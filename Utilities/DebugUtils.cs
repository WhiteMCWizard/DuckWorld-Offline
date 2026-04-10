using UnityEngine;

public static class DebugUtils
{
	public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float duration = 0f, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20f)
	{
		Debug.DrawRay(pos, direction, color, duration);
		Vector3 vector = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f + arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
		Vector3 vector2 = Quaternion.LookRotation(direction) * Quaternion.Euler(0f, 180f - arrowHeadAngle, 0f) * new Vector3(0f, 0f, 1f);
		Debug.DrawRay(pos + direction, vector * arrowHeadLength, color, duration);
		Debug.DrawRay(pos + direction, vector2 * arrowHeadLength, color, duration);
	}
}

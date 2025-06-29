using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Berend/Show Trail")]
public class ShowTrail : MonoBehaviour
{
	private struct TimePoint
	{
		public Vector3 pos;

		public float t;
	}

	[SerializeField]
	private Color col = Color.magenta;

	[SerializeField]
	private bool showTimeStamps;

	private Vector3 oldPos;

	private List<TimePoint> timePoints = new List<TimePoint>();

	private float nextTargetTime;

	private void Awake()
	{
		Object.Destroy(this);
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup > nextTargetTime)
		{
			timePoints.Add(new TimePoint
			{
				pos = base.transform.position,
				t = Time.realtimeSinceStartup
			});
			nextTargetTime += 0.2f;
		}
		for (int i = 1; i < timePoints.Count; i++)
		{
			Debug.DrawLine(timePoints[i].pos, timePoints[i - 1].pos, col);
		}
	}

	private void OnGUI()
	{
		int num = 0;
		while (showTimeStamps && Camera.main != null && num < timePoints.Count)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(timePoints[num].pos);
			vector.y = (float)Screen.height - vector.y;
			if (!(vector.z < 0f))
			{
				GUI.Label(new Rect(vector.x, vector.y, 200f, 200f), StringFormatter.GetFormattedTime(timePoints[num].t, miliseconds: true));
			}
			num++;
		}
	}
}

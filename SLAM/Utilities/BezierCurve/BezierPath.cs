using System;
using System.Collections.Generic;
using UnityEngine;

namespace SLAM.Utilities.BezierCurve;

public class BezierPath : MonoBehaviour
{
	[Serializable]
	public class PathPoint
	{
		public Vector3 p1;

		public Vector3 h1;

		[HideInInspector]
		public Bezier bezier = new Bezier(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero);

		[HideInInspector]
		public float distance;

		[HideInInspector]
		public float[] internalDistance = new float[10];
	}

	public Color color = Color.yellow;

	public Color lineColor = Color.red;

	public List<PathPoint> points = new List<PathPoint>();

	public float TotalDistance { get; protected set; }

	private void Awake()
	{
		for (int i = 0; i < points.Count; i++)
		{
			points[i].bezier.p1 = points[i].p1;
			points[i].bezier.h1 = points[i].h1;
			points[i].bezier.p2 = points[(i + 1) % points.Count].p1;
			points[i].bezier.h2 = -points[(i + 1) % points.Count].h1;
			points[i].internalDistance = new float[10];
			for (int j = 0; j < 10; j++)
			{
				points[i].internalDistance[j] = (points[i].bezier.GetPointAtTime((float)j / 10f + 0.1f) - points[i].bezier.GetPointAtTime((float)j / 10f)).magnitude;
				points[i].distance += points[i].internalDistance[j];
			}
			TotalDistance += points[i].distance;
		}
	}

	public Vector3 GetPositionByT(float t)
	{
		if (t >= (float)(points.Count - 1))
		{
			return points[points.Count - 2].bezier.GetPointAtTime(1f);
		}
		if (t <= 0f)
		{
			return points[0].bezier.GetPointAtTime(0f);
		}
		int num = (int)t;
		return points[num].bezier.GetPointAtTime(t - (float)num);
	}

	public Vector3 GetPositionByDistance(float dist)
	{
		if (Mathf.Approximately(dist, 0f))
		{
			return points[0].bezier.GetPointAtTime(0f);
		}
		while (dist < 0f)
		{
			dist += TotalDistance;
		}
		while (dist > TotalDistance)
		{
			dist -= TotalDistance;
		}
		int num = 0;
		while (dist > 0f && num < points.Count)
		{
			dist -= points[num].distance;
			num++;
		}
		num--;
		dist += points[num].distance;
		int num2 = 0;
		while (dist > 0f && num2 < points[num].internalDistance.Length - 1)
		{
			dist -= points[num].internalDistance[num2];
			num2++;
		}
		num2--;
		dist += points[num].internalDistance[num2];
		float num3 = dist / points[num].internalDistance[num2] / 10f;
		num3 += (float)num2 / 10f;
		return points[num].bezier.GetPointAtTime(num3);
	}

	public float GetDistanceByPosition(Vector3 position)
	{
		float num = TotalDistance / 100f;
		float num2 = TotalDistance;
		float result = 0f;
		for (float num3 = 0f; num3 < TotalDistance; num3 += num)
		{
			float num4 = Vector3.SqrMagnitude(GetPositionByDistance(num3) - position);
			if (num4 < num2)
			{
				num2 = num4;
				result = num3;
			}
		}
		return result;
	}
}

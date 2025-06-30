using System;
using UnityEngine;

namespace SLAM.Utilities.BezierCurve;

[Serializable]
public class Bezier
{
	public Vector3 p1;

	public Vector3 h1;

	public Vector3 h2;

	public Vector3 p2;

	private Vector3 b0 = Vector3.zero;

	private Vector3 b1 = Vector3.zero;

	private Vector3 b2 = Vector3.zero;

	private Vector3 b3 = Vector3.zero;

	private float Ax;

	private float Ay;

	private float Az;

	private float Bx;

	private float By;

	private float Bz;

	private float Cx;

	private float Cy;

	private float Cz;

	public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		p1 = v0;
		h1 = v1;
		h2 = v2;
		p2 = v3;
	}

	public Vector3 GetPointAtTime(float t)
	{
		CheckConstant();
		float num = t * t;
		float num2 = t * t * t;
		float x = Ax * num2 + Bx * num + Cx * t + p1.x;
		float y = Ay * num2 + By * num + Cy * t + p1.y;
		float z = Az * num2 + Bz * num + Cz * t + p1.z;
		return new Vector3(x, y, z);
	}

	private void SetConstant()
	{
		Cx = 3f * (p1.x + h1.x - p1.x);
		Bx = 3f * (p2.x + h2.x - (p1.x + h1.x)) - Cx;
		Ax = p2.x - p1.x - Cx - Bx;
		Cy = 3f * (p1.y + h1.y - p1.y);
		By = 3f * (p2.y + h2.y - (p1.y + h1.y)) - Cy;
		Ay = p2.y - p1.y - Cy - By;
		Cz = 3f * (p1.z + h1.z - p1.z);
		Bz = 3f * (p2.z + h2.z - (p1.z + h1.z)) - Cz;
		Az = p2.z - p1.z - Cz - Bz;
	}

	private void CheckConstant()
	{
		if (p1 != b0 || h1 != b1 || h2 != b2 || p2 != b3)
		{
			SetConstant();
			b0 = p1;
			b1 = h1;
			b2 = h2;
			b3 = p2;
		}
	}
}

using System;
using UnityEngine;

namespace SLAM.CraneOperator;

[Serializable]
public class RayOrigin
{
	public Ray Ray;

	public Direction Direction;

	public RayOrigin(Ray r, Direction d)
	{
		Ray = r;
		Direction = d;
	}
}

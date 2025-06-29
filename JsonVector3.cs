using System;
using UnityEngine;

[Serializable]
public struct JsonVector3
{
	public float x;

	public float y;

	public float z;

	public static implicit operator JsonVector3(Vector3 v3)
	{
		return new JsonVector3
		{
			x = v3.x,
			y = v3.y,
			z = v3.z
		};
	}

	public static implicit operator Vector3(JsonVector3 jv3)
	{
		return new Vector3(jv3.x, jv3.y, jv3.z);
	}
}

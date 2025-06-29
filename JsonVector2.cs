using System;
using UnityEngine;

[Serializable]
public struct JsonVector2
{
	public float x;

	public float y;

	public static implicit operator JsonVector2(Vector2 v2)
	{
		return new JsonVector2
		{
			x = v2.x,
			y = v2.y
		};
	}

	public static implicit operator Vector2(JsonVector2 jv2)
	{
		return new Vector2(jv2.x, jv2.y);
	}
}

using System;
using UnityEngine;

[Serializable]
public struct JsonColor
{
	public float r;

	public float g;

	public float b;

	public static implicit operator JsonColor(Color col)
	{
		return new JsonColor
		{
			r = col.r,
			g = col.g,
			b = col.b
		};
	}

	public static implicit operator Color(JsonColor jcol)
	{
		return new Color(jcol.r, jcol.g, jcol.b);
	}
}

using System;
using UnityEngine;

public class BitMaskAttribute : PropertyAttribute
{
	public Type propType;

	public BitMaskAttribute(Type aType)
	{
		propType = aType;
	}
}

using System;
using UnityEngine;

namespace CinemaDirector;

public static class UnityPropertyTypeInfo
{
	public static PropertyTypeInfo GetMappedType(Type type)
	{
		if (type == typeof(int))
		{
			return PropertyTypeInfo.Int;
		}
		if (type == typeof(long))
		{
			return PropertyTypeInfo.Long;
		}
		if (type == typeof(float))
		{
			return PropertyTypeInfo.Float;
		}
		if (type == typeof(double))
		{
			return PropertyTypeInfo.Double;
		}
		if (type == typeof(Vector2))
		{
			return PropertyTypeInfo.Vector2;
		}
		if (type == typeof(Vector3))
		{
			return PropertyTypeInfo.Vector3;
		}
		if (type == typeof(Vector4))
		{
			return PropertyTypeInfo.Vector4;
		}
		if (type == typeof(Quaternion))
		{
			return PropertyTypeInfo.Quaternion;
		}
		if (type == typeof(Color))
		{
			return PropertyTypeInfo.Color;
		}
		return PropertyTypeInfo.None;
	}

	public static int GetCurveCount(int p)
	{
		return GetCurveCount((PropertyTypeInfo)p);
	}

	public static int GetCurveCount(PropertyTypeInfo info)
	{
		switch (info)
		{
		case PropertyTypeInfo.Double:
		case PropertyTypeInfo.Float:
		case PropertyTypeInfo.Int:
		case PropertyTypeInfo.Long:
			return 1;
		case PropertyTypeInfo.Vector2:
			return 2;
		case PropertyTypeInfo.Vector3:
			return 3;
		case PropertyTypeInfo.Color:
		case PropertyTypeInfo.Quaternion:
		case PropertyTypeInfo.Vector4:
			return 4;
		default:
			return 0;
		}
	}

	public static Color GetCurveColor(int i)
	{
		Color result = Color.white;
		switch (i)
		{
		case 0:
			result = Color.red;
			break;
		case 1:
			result = Color.green;
			break;
		case 2:
			result = Color.blue;
			break;
		case 3:
			result = Color.yellow;
			break;
		}
		return result;
	}

	public static Color GetCurveColor(string Type, string PropertyName, string label, int i)
	{
		Color result = Color.white;
		if (Type == "Transform")
		{
			if (PropertyName == "localPosition" || PropertyName == "position")
			{
				if (label == "x" || i == 0)
				{
					result = Color.red;
				}
				if (label == "y" || i == 1)
				{
					result = Color.green;
				}
				if (label == "z" || i == 2)
				{
					result = Color.blue;
				}
			}
			if (PropertyName == "localEulerAngles")
			{
				if (label == "x" || i == 0)
				{
					result = Color.magenta;
				}
				if (label == "y" || i == 1)
				{
					result = Color.yellow;
				}
				if (label == "z" || i == 2)
				{
					result = Color.cyan;
				}
			}
			if (PropertyName == "localScale")
			{
				if (label == "x" || i == 0)
				{
					result = new Color(0.6745f, 0.4392f, 0.4588f, 1f);
				}
				if (label == "y" || i == 1)
				{
					result = new Color(0.447f, 0.6196f, 0.4588f, 1f);
				}
				if (label == "z" || i == 2)
				{
					result = new Color(0.447f, 0.4392f, 0.7294f, 1f);
				}
			}
		}
		else
		{
			result = GetCurveColor(i);
		}
		return result;
	}

	public static string GetCurveName(PropertyTypeInfo info, int i)
	{
		string result = "x";
		switch (i)
		{
		case 1:
			result = "y";
			break;
		case 2:
			result = "z";
			break;
		case 3:
			result = "w";
			break;
		}
		switch (info)
		{
		case PropertyTypeInfo.Double:
		case PropertyTypeInfo.Float:
		case PropertyTypeInfo.Int:
		case PropertyTypeInfo.Long:
			result = "value";
			break;
		case PropertyTypeInfo.Color:
			switch (i)
			{
			case 0:
				result = "r";
				break;
			case 1:
				result = "g";
				break;
			case 2:
				result = "b";
				break;
			case 3:
				result = "a";
				break;
			}
			break;
		}
		return result;
	}

	public static Type GetUnityType(string typeName)
	{
		return typeName switch
		{
			"Transform" => typeof(Transform), 
			"Camera" => typeof(Camera), 
			"Light" => typeof(Light), 
			_ => typeof(Transform), 
		};
	}
}

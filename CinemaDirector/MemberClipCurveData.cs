using System;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector;

[Serializable]
public class MemberClipCurveData
{
	public string Type;

	public string PropertyName;

	public bool IsProperty = true;

	public PropertyTypeInfo PropertyType = PropertyTypeInfo.None;

	public AnimationCurve Curve1 = new AnimationCurve();

	public AnimationCurve Curve2 = new AnimationCurve();

	public AnimationCurve Curve3 = new AnimationCurve();

	public AnimationCurve Curve4 = new AnimationCurve();

	public AnimationCurve GetCurve(int i)
	{
		return i switch
		{
			1 => Curve2, 
			2 => Curve3, 
			3 => Curve4, 
			_ => Curve1, 
		};
	}

	public void SetCurve(int i, AnimationCurve newCurve)
	{
		switch (i)
		{
		case 1:
			Curve2 = newCurve;
			break;
		case 2:
			Curve3 = newCurve;
			break;
		case 3:
			Curve4 = newCurve;
			break;
		default:
			Curve1 = newCurve;
			break;
		}
	}

	public void Initialize(GameObject Actor)
	{
	}

	internal void Reset(GameObject Actor)
	{
	}

	internal object getCurrentValue(Component component)
	{
		if (component == null || PropertyName == string.Empty)
		{
			return null;
		}
		Type type = component.GetType();
		object obj = null;
		if (IsProperty)
		{
			PropertyInfo property = type.GetProperty(PropertyName);
			return property.GetValue(component, null);
		}
		FieldInfo field = type.GetField(PropertyName);
		return field.GetValue(component);
	}
}

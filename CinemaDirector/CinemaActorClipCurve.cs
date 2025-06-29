using System;
using System.Collections.Generic;
using System.Reflection;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[Serializable]
[CutsceneItem("Curve Clip", "Actor Curve Clip", new CutsceneItemGenre[] { CutsceneItemGenre.CurveClipItem })]
public class CinemaActorClipCurve : CinemaClipCurve, IRevertable
{
	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	public GameObject Actor
	{
		get
		{
			GameObject result = null;
			if (base.transform.parent != null)
			{
				CurveTrack component = base.transform.parent.GetComponent<CurveTrack>();
				if (component != null && component.Actor != null)
				{
					result = component.Actor.gameObject;
				}
			}
			return result;
		}
	}

	public RevertMode EditorRevertMode
	{
		get
		{
			return editorRevertMode;
		}
		set
		{
			editorRevertMode = value;
		}
	}

	public RevertMode RuntimeRevertMode
	{
		get
		{
			return runtimeRevertMode;
		}
		set
		{
			runtimeRevertMode = value;
		}
	}

	protected override void initializeClipCurves(MemberClipCurveData data, Component component)
	{
		object currentValue = GetCurrentValue(component, data.PropertyName, data.IsProperty);
		PropertyTypeInfo propertyType = data.PropertyType;
		float timeStart = base.Firetime;
		float timeEnd = base.Firetime + base.Duration;
		switch (propertyType)
		{
		case PropertyTypeInfo.Double:
		case PropertyTypeInfo.Float:
		case PropertyTypeInfo.Int:
		case PropertyTypeInfo.Long:
		{
			float num = (float)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, num, timeEnd, num);
			break;
		}
		case PropertyTypeInfo.Vector2:
		{
			Vector2 vector3 = (Vector2)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, vector3.x, timeEnd, vector3.x);
			data.Curve2 = AnimationCurve.Linear(timeStart, vector3.y, timeEnd, vector3.y);
			break;
		}
		case PropertyTypeInfo.Vector3:
		{
			Vector3 vector2 = (Vector3)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, vector2.x, timeEnd, vector2.x);
			data.Curve2 = AnimationCurve.Linear(timeStart, vector2.y, timeEnd, vector2.y);
			data.Curve3 = AnimationCurve.Linear(timeStart, vector2.z, timeEnd, vector2.z);
			break;
		}
		case PropertyTypeInfo.Vector4:
		{
			Vector4 vector = (Vector4)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, vector.x, timeEnd, vector.x);
			data.Curve2 = AnimationCurve.Linear(timeStart, vector.y, timeEnd, vector.y);
			data.Curve3 = AnimationCurve.Linear(timeStart, vector.z, timeEnd, vector.z);
			data.Curve4 = AnimationCurve.Linear(timeStart, vector.w, timeEnd, vector.w);
			break;
		}
		case PropertyTypeInfo.Quaternion:
		{
			Quaternion quaternion = (Quaternion)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, quaternion.x, timeEnd, quaternion.x);
			data.Curve2 = AnimationCurve.Linear(timeStart, quaternion.y, timeEnd, quaternion.y);
			data.Curve3 = AnimationCurve.Linear(timeStart, quaternion.z, timeEnd, quaternion.z);
			data.Curve4 = AnimationCurve.Linear(timeStart, quaternion.w, timeEnd, quaternion.w);
			break;
		}
		case PropertyTypeInfo.Color:
		{
			Color color = (Color)currentValue;
			data.Curve1 = AnimationCurve.Linear(timeStart, color.r, timeEnd, color.r);
			data.Curve2 = AnimationCurve.Linear(timeStart, color.g, timeEnd, color.g);
			data.Curve3 = AnimationCurve.Linear(timeStart, color.b, timeEnd, color.b);
			data.Curve4 = AnimationCurve.Linear(timeStart, color.a, timeEnd, color.a);
			break;
		}
		}
	}

	public object GetCurrentValue(Component component, string propertyName, bool isProperty)
	{
		if (component == null || propertyName == string.Empty)
		{
			return null;
		}
		Type type = component.GetType();
		object obj = null;
		if (isProperty)
		{
			PropertyInfo property = type.GetProperty(propertyName);
			return property.GetValue(component, null);
		}
		FieldInfo field = type.GetField(propertyName);
		return field.GetValue(component);
	}

	public override void Initialize()
	{
		foreach (MemberClipCurveData curveDatum in base.CurveData)
		{
			curveDatum.Initialize(Actor);
		}
	}

	public RevertInfo[] CacheState()
	{
		List<RevertInfo> list = new List<RevertInfo>();
		if (Actor != null)
		{
			foreach (MemberClipCurveData curveDatum in base.CurveData)
			{
				Component component = Actor.GetComponent(curveDatum.Type);
				list.Add(new RevertInfo(this, component, curveDatum.PropertyName, curveDatum.getCurrentValue(component)));
			}
		}
		return list.ToArray();
	}

	public void SampleTime(float time)
	{
		if (Actor == null || !(base.Firetime <= time) || !(time <= base.Firetime + base.Duration))
		{
			return;
		}
		foreach (MemberClipCurveData curveDatum in base.CurveData)
		{
			if (!(curveDatum.Type == string.Empty) && !(curveDatum.PropertyName == string.Empty))
			{
				Component component = Actor.GetComponent(curveDatum.Type);
				Type type = component.GetType();
				object value = evaluate(curveDatum, time);
				if (curveDatum.IsProperty)
				{
					PropertyInfo property = type.GetProperty(curveDatum.PropertyName);
					property.SetValue(component, value, null);
				}
				else
				{
					FieldInfo field = type.GetField(curveDatum.PropertyName);
					field.SetValue(component, value);
				}
			}
		}
	}

	internal void Reset()
	{
	}
}

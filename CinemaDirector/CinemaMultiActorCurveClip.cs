using System;
using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[Serializable]
[CutsceneItem("Curve Clip", "MultiActor Curve Clip", new CutsceneItemGenre[] { CutsceneItemGenre.MultiActorCurveClipItem })]
public class CinemaMultiActorCurveClip : CinemaClipCurve, IRevertable
{
	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	public List<Component> Components = new List<Component>();

	public List<string> Properties = new List<string>();

	public List<Transform> Actors
	{
		get
		{
			List<Transform> result = new List<Transform>();
			if (base.transform.parent != null)
			{
				MultiCurveTrack component = base.transform.parent.GetComponent<MultiCurveTrack>();
				MultiActorTrackGroup multiActorTrackGroup = component.TrackGroup as MultiActorTrackGroup;
				result = multiActorTrackGroup.Actors;
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

	public CinemaMultiActorCurveClip()
	{
		base.CurveData.Add(new MemberClipCurveData());
	}

	public void SampleTime(float time)
	{
		if (!(base.Firetime <= time) || !(time <= base.Firetime + base.Duration))
		{
			return;
		}
		MemberClipCurveData memberClipCurveData = base.CurveData[0];
		if (memberClipCurveData == null || memberClipCurveData.PropertyType == PropertyTypeInfo.None)
		{
			return;
		}
		Color color = default(Color);
		Quaternion quaternion = default(Quaternion);
		Vector2 vector3 = default(Vector2);
		Vector3 vector2 = default(Vector3);
		Vector4 vector = default(Vector4);
		for (int i = 0; i < Components.Count; i++)
		{
			object value = null;
			switch (memberClipCurveData.PropertyType)
			{
			case PropertyTypeInfo.Color:
				color.r = memberClipCurveData.Curve1.Evaluate(time);
				color.g = memberClipCurveData.Curve2.Evaluate(time);
				color.b = memberClipCurveData.Curve3.Evaluate(time);
				color.a = memberClipCurveData.Curve4.Evaluate(time);
				value = color;
				break;
			case PropertyTypeInfo.Double:
			case PropertyTypeInfo.Float:
			case PropertyTypeInfo.Int:
			case PropertyTypeInfo.Long:
				value = memberClipCurveData.Curve1.Evaluate(time);
				break;
			case PropertyTypeInfo.Quaternion:
				quaternion.x = memberClipCurveData.Curve1.Evaluate(time);
				quaternion.y = memberClipCurveData.Curve2.Evaluate(time);
				quaternion.z = memberClipCurveData.Curve3.Evaluate(time);
				quaternion.w = memberClipCurveData.Curve4.Evaluate(time);
				value = quaternion;
				break;
			case PropertyTypeInfo.Vector2:
				vector3.x = memberClipCurveData.Curve1.Evaluate(time);
				vector3.y = memberClipCurveData.Curve2.Evaluate(time);
				value = vector3;
				break;
			case PropertyTypeInfo.Vector3:
				vector2.x = memberClipCurveData.Curve1.Evaluate(time);
				vector2.y = memberClipCurveData.Curve2.Evaluate(time);
				vector2.z = memberClipCurveData.Curve3.Evaluate(time);
				value = vector2;
				break;
			case PropertyTypeInfo.Vector4:
				vector.x = memberClipCurveData.Curve1.Evaluate(time);
				vector.y = memberClipCurveData.Curve2.Evaluate(time);
				vector.z = memberClipCurveData.Curve3.Evaluate(time);
				vector.w = memberClipCurveData.Curve4.Evaluate(time);
				value = vector;
				break;
			}
			if (Components[i] != null && Properties[i] != null && Properties[i] != "None")
			{
				Components[i].GetType().GetProperty(Properties[i]).SetValue(Components[i], value, null);
			}
		}
	}

	public RevertInfo[] CacheState()
	{
		List<RevertInfo> list = new List<RevertInfo>();
		for (int i = 0; i < Actors.Count; i++)
		{
			if (i < Components.Count && i < Properties.Count && Components[i] != null && Properties[i] != null && Properties[i] != "None")
			{
				Component obj = Components[i];
				list.Add(new RevertInfo(this, obj, Properties[i], Components[i].GetType().GetProperty(Properties[i]).GetValue(Components[i], null)));
			}
		}
		return list.ToArray();
	}

	internal void Revert()
	{
	}
}

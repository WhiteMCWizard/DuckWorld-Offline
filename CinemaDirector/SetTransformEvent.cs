using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transform", "Set Transform", new CutsceneItemGenre[] { CutsceneItemGenre.TransformItem })]
public class SetTransformEvent : CinemaActorEvent, IRevertable
{
	public Transform Transform;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

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

	public RevertInfo[] CacheState()
	{
		List<Transform> list = new List<Transform>(GetActors());
		List<RevertInfo> list2 = new List<RevertInfo>();
		foreach (Transform item in list)
		{
			if (item != null)
			{
				Transform component = item.GetComponent<Transform>();
				if (component != null)
				{
					list2.Add(new RevertInfo(this, component, "localPosition", component.localPosition));
					list2.Add(new RevertInfo(this, component, "localRotation", component.localRotation));
					list2.Add(new RevertInfo(this, component, "localScale", component.localScale));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			actor.transform.position = Transform.position;
			actor.transform.rotation = Transform.rotation;
			actor.transform.localScale = Transform.localScale;
		}
	}
}

using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Physics", "Toggle Gravity", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ToggleGravityEvent : CinemaActorEvent, IRevertable
{
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
				Rigidbody component = item.GetComponent<Rigidbody>();
				if (component != null)
				{
					list2.Add(new RevertInfo(this, component, "useGravity", component.useGravity));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		Rigidbody component = actor.GetComponent<Rigidbody>();
		if (component != null)
		{
			component.useGravity = !component.useGravity;
		}
	}

	public override void Reverse(GameObject actor)
	{
		Trigger(actor);
	}
}

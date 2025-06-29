using System.Collections.Generic;
using System.Reflection;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Enable Behaviour", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class EnableBehaviour : CinemaActorEvent, IRevertable
{
	public Component Behaviour;

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
				Component component = item.GetComponent(Behaviour.GetType());
				if (component != null)
				{
					PropertyInfo property = Behaviour.GetType().GetProperty("enabled");
					bool flag = (bool)property.GetValue(component, null);
					list2.Add(new RevertInfo(this, Behaviour, "enabled", flag));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		Component component = actor.GetComponent(Behaviour.GetType());
		if (component != null)
		{
			Behaviour.GetType().GetMember("enabled");
			PropertyInfo property = Behaviour.GetType().GetProperty("enabled");
			property.SetValue(Behaviour, true, null);
		}
	}

	public override void Reverse(GameObject actor)
	{
		Component component = actor.GetComponent(Behaviour.GetType());
		if (component != null)
		{
			PropertyInfo property = Behaviour.GetType().GetProperty("enabled");
			property.SetValue(component, false, null);
		}
	}
}

using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Light", "Set Light Colour", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class SetLightColour : CinemaActorEvent, IRevertable
{
	public Color Color;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private Color previousColor;

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
				Light component = item.GetComponent<Light>();
				if (component != null)
				{
					list2.Add(new RevertInfo(this, component, "color", component.color));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Initialize(GameObject actor)
	{
		Light component = actor.GetComponent<Light>();
		if (component != null)
		{
			previousColor = component.color;
		}
	}

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			Light component = actor.GetComponent<Light>();
			if (component != null)
			{
				previousColor = component.color;
				component.color = Color;
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
		if (actor != null)
		{
			Light component = actor.GetComponent<Light>();
			if (component != null)
			{
				component.color = previousColor;
			}
		}
	}
}

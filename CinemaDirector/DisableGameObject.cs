using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Disable", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class DisableGameObject : CinemaActorEvent, IRevertable
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
				list2.Add(new RevertInfo(this, item.gameObject, "SetActive", item.gameObject.activeSelf));
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		if (actor != null)
		{
			actor.SetActive(value: false);
		}
	}

	public override void Reverse(GameObject actor)
	{
		if (actor != null)
		{
			actor.SetActive(value: true);
		}
	}
}

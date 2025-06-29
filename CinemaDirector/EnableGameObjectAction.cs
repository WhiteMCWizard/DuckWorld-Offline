using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Temporary Enable", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class EnableGameObjectAction : CinemaActorAction, IRevertable
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
			actor.SetActive(value: true);
		}
	}

	public override void End(GameObject actor)
	{
		if (actor != null)
		{
			actor.SetActive(value: false);
		}
	}

	public override void ReverseTrigger(GameObject Actor)
	{
		End(Actor);
	}

	public override void ReverseEnd(GameObject Actor)
	{
		Trigger(Actor);
	}

	public override void SetTime(GameObject actor, float time, float deltaTime)
	{
		if (actor != null)
		{
			actor.SetActive(time >= 0f && time < base.Duration);
		}
	}
}

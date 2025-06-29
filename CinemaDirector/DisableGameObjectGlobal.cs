using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Disable Game Object", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class DisableGameObjectGlobal : CinemaGlobalEvent, IRevertable
{
	public GameObject target;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private bool previousState;

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
		if (target != null)
		{
			return new RevertInfo[1]
			{
				new RevertInfo(this, target, "SetActive", target.activeInHierarchy)
			};
		}
		return null;
	}

	public override void Trigger()
	{
		if (target != null)
		{
			previousState = target.activeInHierarchy;
			target.SetActive(value: false);
		}
	}

	public override void Reverse()
	{
		if (target != null)
		{
			target.SetActive(previousState);
		}
	}
}

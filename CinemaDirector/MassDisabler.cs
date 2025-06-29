using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Utility", "Mass Disabler", new CutsceneItemGenre[] { CutsceneItemGenre.GlobalItem })]
public class MassDisabler : CinemaGlobalAction, IRevertable
{
	public List<GameObject> GameObjects = new List<GameObject>();

	public List<string> Tags = new List<string>();

	private List<GameObject> tagsCache = new List<GameObject>();

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
		List<GameObject> list = new List<GameObject>();
		foreach (string tag in Tags)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
			GameObject[] array2 = array;
			foreach (GameObject item in array2)
			{
				list.Add(item);
			}
		}
		list.AddRange(GameObjects);
		List<RevertInfo> list2 = new List<RevertInfo>();
		foreach (GameObject item2 in list)
		{
			if (item2 != null)
			{
				list2.Add(new RevertInfo(this, item2, "SetActive", item2.activeInHierarchy));
			}
		}
		return list2.ToArray();
	}

	public override void Trigger()
	{
		tagsCache.Clear();
		foreach (string tag in Tags)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
			GameObject[] array2 = array;
			foreach (GameObject item in array2)
			{
				tagsCache.Add(item);
			}
		}
		setActive(enabled: false);
	}

	public override void End()
	{
		setActive(enabled: true);
	}

	public override void ReverseTrigger()
	{
		End();
	}

	public override void ReverseEnd()
	{
		Trigger();
	}

	private void setActive(bool enabled)
	{
		foreach (GameObject gameObject in GameObjects)
		{
			gameObject.SetActive(enabled);
		}
		foreach (GameObject item in tagsCache)
		{
			item.SetActive(enabled);
		}
	}
}

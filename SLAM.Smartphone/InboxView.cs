using System.Collections.Generic;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class InboxView : AppView
{
	[SerializeField]
	private UIGrid inboxGrid;

	[SerializeField]
	private GameObject inboxItemPrefab;

	private List<Message> messages;

	public void SetData(List<Message> data)
	{
		messages = data;
		createInboxItems(messages);
	}

	public override void ReturnFromBackground()
	{
		base.ReturnFromBackground();
		createInboxItems(messages);
	}

	private void createInboxItems(List<Message> messages)
	{
		inboxGrid.transform.DestroyChildren();
		inboxGrid.transform.DetachChildren();
		for (int i = 0; i < messages.Count; i++)
		{
			GameObject gameObject = NGUITools.AddChild(inboxGrid.gameObject, inboxItemPrefab);
			gameObject.GetComponent<InboxItemRow>().SetData(messages[i]);
			gameObject.name = string.Concat(messages[i].Type, " ", messages[i].Id, " ", messages[i].Archived);
		}
		inboxGrid.enabled = true;
		inboxGrid.Reposition();
	}
}

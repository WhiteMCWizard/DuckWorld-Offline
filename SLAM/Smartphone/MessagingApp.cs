using System;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
using SLAM.SaveSystem;
using SLAM.Slinq;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class MessagingApp : AppController
{
	private List<Message> allMessages = new List<Message>();

	private List<Message> unreadMessages = new List<Message>();

	public override int NotificationCount
	{
		get
		{
			base.NotificationCount = unreadMessages.Count;
			return base.NotificationCount;
		}
		protected set
		{
			base.NotificationCount = value;
		}
	}

	protected override void Start()
	{
		base.Start();
		// Load messages initially
		if (SaveManager.Instance.IsLoaded)
		{
			LoadMessagesFromLocal();
		}
		InvokeRepeating("refreshNotifications", 0f, 10f);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<OpenMessageEvent>(onOpenMessageRequest);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<OpenMessageEvent>(onOpenMessageRequest);
	}

	public override void Open()
	{
		LoadMessagesFromLocal();
		OpenView<InboxView>().SetData(allMessages);
	}
	
	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
		if (UserProfile.Current.IsFree)
		{
			return;
		}

		// Get messages from local storage instead of API
		LoadMessagesFromLocal();

		unreadMessages = allMessages.Where((Message m) => !m.Archived && m.Type != Message.MessageType.JobNotification).ToList();
		if (unreadMessages.Count > 0)
		{
			eventCallback(new AppChangedEvent
			{
				App = this
			});
		}
	}

	private void LoadMessagesFromLocal()
	{
		if (SaveManager.Instance.IsLoaded)
		{
			var saveData = SaveManager.Instance.GetSaveData();
			if (saveData.messages != null && saveData.messages.Length > 0)
			{
				allMessages = saveData.messages.Where((Message m) => m.Type != Message.MessageType.JobNotification).ToList();
			}
			else
			{
				CreateDefaultMessagesIfNeeded();
				allMessages = saveData.messages?.Where((Message m) => m.Type != Message.MessageType.JobNotification).ToList() ?? new List<Message>();
			}
		}
		else
		{
			allMessages = new List<Message>();
		}
	}

	private void SaveMessagesToLocal()
	{
		if (SaveManager.Instance.IsLoaded)
		{
			var saveData = SaveManager.Instance.GetSaveData();
			saveData.messages = allMessages.ToArray();
			SaveManager.Instance.MarkDirty();
		}
	}

	private void CreateDefaultMessagesIfNeeded()
	{
		if (SaveManager.Instance.IsLoaded)
		{
			var saveData = SaveManager.Instance.GetSaveData();
			if (saveData.messages == null || saveData.messages.Length == 0)
			{
				// Create some default messages for demonstration
				var defaultMessages = new List<Message>();
				
				// Add a welcome message
				var welcomeMessage = new Message
				{
					Id = 1,
					Type = Message.MessageType.Notification,
					Sender = new UserProfile { Name = "System", Id = 0 },
					MessageBody = "Test",
					Archived = false,
					dateCreated = DateTime.Now.ToString(),
					dateModified = DateTime.Now.ToString()
				};
				defaultMessages.Add(welcomeMessage);
				
				saveData.messages = defaultMessages.ToArray();
				SaveManager.Instance.MarkDirty();
				allMessages = defaultMessages;
			}
		}
	}

	public void AddMessage(Message message)
	{
		if (message == null) return;
		
		LoadMessagesFromLocal();
		
		// Assign a unique ID if not set
		if (message.Id == 0)
		{
			int maxId = 0;
			foreach (var m in allMessages)
			{
				if (m.Id > maxId) maxId = m.Id;
			}
			message.Id = maxId + 1;
		}
		
		// Set creation and modification dates if not set
		if (string.IsNullOrEmpty(message.dateCreated))
		{
			message.dateCreated = DateTime.Now.ToString();
		}
		if (string.IsNullOrEmpty(message.dateModified))
		{
			message.dateModified = DateTime.Now.ToString();
		}
		
		allMessages.Add(message);
		SaveMessagesToLocal();
	}

	private void onOpenMessageRequest(OpenMessageEvent evt)
	{
		switch (evt.Message.Type)
		{
		default:
			openGlobalNotification(evt.Message);
			archiveMessage(evt.Message);
			break;
		case Message.MessageType.Notification:
			openNotification(evt.Message);
			archiveMessage(evt.Message);
			break;
		case Message.MessageType.UrlMessage:
			openUrlMessage(evt.Message);
			archiveMessage(evt.Message);
			break;
		}
	}

	private void archiveMessage(Message message)
	{
		message.Archived = true;
		if (message.Type != Message.MessageType.JobNotification && message.Type != Message.MessageType.GlobalNotification)
		{
			// Save to local storage instead of API
			SaveMessagesToLocal();
		}
		
		Message message2 = unreadMessages.FirstOrDefault((Message m) => m.Id == message.Id);
		if (message2 != null && unreadMessages.Contains(message2))
		{
			unreadMessages.Remove(message2);
			AppChangedEvent appChangedEvent = new AppChangedEvent();
			appChangedEvent.App = this;
			GameEvents.Invoke(appChangedEvent);
		}
	}

	private void openGlobalNotification(Message m)
	{
		string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_GLOBALNOTIFICATION", m.Sender.Name);
		string messageBody = m.MessageBody;
		OpenView<InboxItemView>().SetData(localizationFormatted, messageBody, m, null, null);
	}

	private void openNotification(Message m)
	{
		string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_NOTIFICATION", m.Sender.Name);
		string messageBody = m.MessageBody;
		OpenView<InboxItemView>().SetData(localizationFormatted, messageBody, m, null, null);
	}

	private void openUrlMessage(Message message)
	{
		string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_URLMESSAGE", message.Sender.Name);
		string messageBody = message.MessageBody;
		OpenView<InboxItemView>().SetData(localizationFormatted, messageBody, message, delegate
		{
			Application.OpenURL(message.Url);
		}, delegate
		{
		});
	}
}

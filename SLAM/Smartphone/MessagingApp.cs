using System;
using System.Collections.Generic;
using SLAM.Analytics;
using SLAM.BuildSystem;
using SLAM.Engine;
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
		OpenTempView<LoadingView>();
		Webservice.WaitFor(onMessagesAndMugshotsRecieved, from m in allMessages
			select m.Sender.MugShotUrl into mugshoturl
			where !string.IsNullOrEmpty(mugshoturl)
			select mugshoturl);
	}

	private void onMessagesAndMugshotsRecieved()
	{
		if (IsViewOpen<LoadingView>())
		{
			CloseTempView<LoadingView>();
			OpenView<InboxView>().SetData(allMessages);
		}
	}

	public void CloseFriendRequest(Message message, bool accepted)
	{
		if (accepted)
		{
			ApiClient.AcceptFriendRequest(message.Id, delegate
			{
				DataStorage.GetFriends(null, forceRefresh: true);
				GameEvents.Invoke(new TrackingEvent
				{
					Type = TrackingEvent.TrackingType.FriendshipAccepted,
					Arguments = new Dictionary<string, object>
					{
						{
							"Sender",
							message.Sender.Id
						},
						{
							"Recipient",
							ApiClient.UserId
						}
					}
				});
			});
		}
		else
		{
			TrackingEvent trackingEvent = new TrackingEvent();
			trackingEvent.Type = TrackingEvent.TrackingType.FriendshipRejected;
			trackingEvent.Arguments = new Dictionary<string, object>
			{
				{
					"Sender",
					message.Sender.Id
				},
				{
					"Recipient",
					ApiClient.UserId
				}
			};
			GameEvents.Invoke(trackingEvent);
		}
		archiveMessage(message);
	}

	public void CloseChallengeRequest(Message message, bool accepted)
	{
		if (accepted)
		{
			GameController.ChallengeAccepted = message;
			SceneManager.Load(message.Game.SceneName);
			Close();
			return;
		}
		TrackingEvent trackingEvent = new TrackingEvent();
		trackingEvent.Type = TrackingEvent.TrackingType.ChallengeRejected;
		trackingEvent.Arguments = new Dictionary<string, object>
		{
			{
				"Sender",
				message.Sender.Id
			},
			{
				"Recipient",
				ApiClient.UserId
			},
			{
				"GameId",
				message.Game.Id
			},
			{ "Difficulty", message.Difficulty }
		};
		GameEvents.Invoke(trackingEvent);
		ApiClient.DeleteMessage(message.Id, null);
	}

	protected override void checkForNotifications(Action<AppChangedEvent> eventCallback)
	{
		if (UserProfile.Current.IsFree)
		{
			return;
		}
		ApiClient.GetAllMessages(delegate(Message[] messages)
		{
			unreadMessages = messages.Where((Message m) => !m.Archived && m.Type != Message.MessageType.JobNotification).ToList();
			allMessages = messages.Where((Message m) => m.Type != Message.MessageType.JobNotification).ToList();
			if (unreadMessages.Count > 0)
			{
				eventCallback(new AppChangedEvent
				{
					App = this
				});
			}
		});
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
		case Message.MessageType.FriendConfirmed:
			openFriendConfirmed(evt.Message);
			archiveMessage(evt.Message);
			break;
		case Message.MessageType.FriendRequest:
			openFriendRequest(evt.Message);
			break;
		case Message.MessageType.Challenge:
			if (evt.Message.ScoreRecipient == 0)
			{
				openChallenge(evt.Message);
			}
			else
			{
				openChallengeResult(evt.Message);
			}
			archiveMessage(evt.Message);
			break;
		case Message.MessageType.ChallengeResult:
			openChallengeResult(evt.Message);
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
		if (!message.Archived && message.Type != Message.MessageType.JobNotification && message.Type != Message.MessageType.GlobalNotification)
		{
			ApiClient.ArchiveMessage(message.Id, null);
		}
		message.Archived = true;
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

	private void openChallenge(Message m)
	{
		string title = Localization.Get("SF_MESSAGING_TITLE_CHALLENGE");
		string body = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGE", m.Sender.Name, Localization.Get(m.Game.Name), m.ScoreSender);
		OpenView<InboxItemView>().SetData(title, body, m, null, null);
		DataStorage.GetProgressionData(delegate(UserGameDetails[] ugd)
		{
			if (IsViewOpen<InboxItemView>())
			{
				if (ugd.Any((UserGameDetails g) => g.GameId == m.Game.Id && g.Progression.Any((UserGameProgression prg) => prg.LevelIndex == m.Difficulty)))
				{
					GetView<InboxItemView>().SetData(title, body, m, delegate
					{
						CloseChallengeRequest(m, accepted: true);
					}, delegate
					{
						CloseChallengeRequest(m, accepted: false);
					});
				}
				else
				{
					body = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGE_LOCKED", m.Sender.Name, Localization.Get(m.Game.Name), m.Difficulty + 1);
					GetView<InboxItemView>().SetData(title, body, m, null, null);
				}
			}
		});
	}

	private void openFriendConfirmed(Message m)
	{
		string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDCONFIRMED", m.Sender.Name);
		string localizationFormatted2 = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_FRIENDCONFIRMED", m.Sender.Name);
		OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, m, null, null);
	}

	private void openChallengeResult(Message m)
	{
		string title = Localization.Get("SF_MESSAGING_TITLE_CHALLENGERESULT");
		string localizationFormatted = StringFormatter.GetLocalizationFormatted((!m.HasRecipientWon()) ? "SF_MESSAGING_BODY_CHALLENGERESULT_LOST" : "SF_MESSAGING_BODY_CHALLENGERESULT_VICTORY", m.Sender.Name, Localization.Get(m.Game.Name), m.ScoreRecipient, m.ScoreSender);
		if (m.WasGameTie())
		{
			localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_CHALLENGERESULT_TIE", m.Sender.Name, Localization.Get(m.Game.Name), m.ScoreRecipient);
		}
		OpenView<InboxItemView>().SetData(title, localizationFormatted, m, null, null);
	}

	private void openFriendRequest(Message m)
	{
		string localizationFormatted = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDREQUEST");
		string localizationFormatted2 = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_BODY_FRIENDREQUEST", m.Sender.Name, m.Sender.Address);
		if (!m.Archived)
		{
			OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, m, delegate
			{
				CloseFriendRequest(m, accepted: true);
			}, delegate
			{
				CloseFriendRequest(m, accepted: false);
			});
		}
		else
		{
			OpenView<InboxItemView>().SetData(localizationFormatted, localizationFormatted2, m, null, null);
		}
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

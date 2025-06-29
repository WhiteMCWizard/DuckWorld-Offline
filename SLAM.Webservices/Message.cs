using System;
using LitJson;

namespace SLAM.Webservices;

[Serializable]
public class Message
{
	public enum MessageType
	{
		GlobalNotification,
		Notification,
		FriendRequest,
		Challenge,
		ChallengeResult,
		JobNotification,
		FriendConfirmed,
		UrlMessage
	}

	[JsonName("id")]
	public int Id;

	[JsonName("messagetype")]
	public MessageType Type;

	[JsonName("sender")]
	public UserProfile Sender;

	[JsonName("recipient")]
	public UserProfile Recipient;

	[JsonName("body")]
	public string MessageBody;

	[JsonName("game")]
	public Game Game;

	[JsonName("difficulty")]
	public int Difficulty;

	[JsonName("archived")]
	public bool Archived;

	[JsonName("updated_at")]
	public string dateModified;

	[JsonName("created_at")]
	public string dateCreated;

	[JsonName("url")]
	public string Url;

	[JsonName("score_sender")]
	public int ScoreSender { get; set; }

	[JsonName("score_recipient")]
	public int ScoreRecipient { get; set; }

	public DateTime DateModified
	{
		get
		{
			if (!string.IsNullOrEmpty(dateModified) && DateTime.TryParse(dateModified, out var result))
			{
				return result;
			}
			return DateTime.Now;
		}
	}

	public bool WasGameTie()
	{
		return ScoreSender == ScoreRecipient;
	}

	public bool HasSenderWon()
	{
		return (Game.Id != 11 && Game.Id != 39) ? (ScoreSender > ScoreRecipient) : (ScoreSender < ScoreRecipient);
	}

	public bool HasRecipientWon()
	{
		return !HasSenderWon();
	}
}

using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class InboxItemRow : MonoBehaviour
{
	[SerializeField]
	private UILabel titleLabel;

	[SerializeField]
	private UILabel dateLabel;

	[SerializeField]
	private UILabel senderLabel;

	[SerializeField]
	private UITexture portraitTexture;

	[SerializeField]
	private Color archivedTitleLabelColor = Color.gray;

	private Message message;

	public void SetData(Message data)
	{
		message = data;
		string text = data.Type.ToString();
		string formattedDate = StringFormatter.GetFormattedDate(data.DateModified);
		switch (data.Type)
		{
		case Message.MessageType.GlobalNotification:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_GLOBALNOTIFICATION");
			break;
		case Message.MessageType.Notification:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_NOTIFICATION", data.Sender.FirstName);
			break;
		case Message.MessageType.FriendRequest:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDREQUEST");
			break;
		case Message.MessageType.FriendConfirmed:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_FRIENDCONFIRMED");
			break;
		case Message.MessageType.Challenge:
			text = ((data.ScoreRecipient != 0) ? StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_CHALLENGERESULT") : StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_CHALLENGE"));
			break;
		case Message.MessageType.ChallengeResult:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_CHALLENGERESULT");
			break;
		case Message.MessageType.JobNotification:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_JOBNOTIFICATION");
			break;
		case Message.MessageType.UrlMessage:
			text = StringFormatter.GetLocalizationFormatted("SF_MESSAGING_TITLE_URLMESSAGE", data.Sender.Name);
			break;
		default:
			Debug.LogErrorFormat("Dont know what to do with message type {0}!", data.Type);
			break;
		}
		titleLabel.text = text;
		dateLabel.text = formattedDate;
		senderLabel.text = ((data.Sender == null) ? Localization.Get("UI_UNKNOWN") : Localization.Get(data.Sender.Name));
		portraitTexture.mainTexture = ((data.Sender == null) ? null : data.Sender.MugShot);
		if (data.Archived)
		{
			titleLabel.color = archivedTitleLabelColor;
		}
	}

	public void OnItemClicked()
	{
		OpenMessageEvent openMessageEvent = new OpenMessageEvent();
		openMessageEvent.Message = message;
		GameEvents.Invoke(openMessageEvent);
	}
}

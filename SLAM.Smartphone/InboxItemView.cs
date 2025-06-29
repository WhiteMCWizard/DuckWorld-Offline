using System;
using SLAM.Shared;
using SLAM.Webservices;
using UnityEngine;

namespace SLAM.Smartphone;

public class InboxItemView : AppView
{
	[SerializeField]
	protected UILabel titleLabel;

	[SerializeField]
	protected UITypewriter bodyTypeWriter;

	[SerializeField]
	protected UILabel senderLabel;

	[SerializeField]
	private UILabel dateLabel;

	[SerializeField]
	private UITexture mugshotTexture;

	[SerializeField]
	private UIButton yesButton;

	[SerializeField]
	private UIButton noButton;

	private UILabel bodyLabel;

	private Action yesCallback;

	private Action noCallback;

	public virtual void SetData(string title, string body, Message m, Action yesCallback, Action noCallback)
	{
		string sender = ((m.Sender == null) ? Localization.Get("UI_UNKNOWN") : m.Sender.Name);
		Texture2D portrait = ((m.Sender == null) ? null : m.Sender.MugShot);
		SetData(title, body, sender, m.DateModified, portrait, yesCallback, noCallback);
	}

	public virtual void SetData(string body, Action yesCallback, Action noCallback)
	{
		bodyTypeWriter.SetText(body);
		this.yesCallback = yesCallback;
		this.noCallback = noCallback;
		yesButton.gameObject.SetActive(this.yesCallback != null);
		noButton.gameObject.SetActive(this.noCallback != null);
	}

	public virtual void SetData(string title, string body, string sender, DateTime date, Texture portrait, Action yesCallback, Action noCallback)
	{
		titleLabel.text = title;
		bodyTypeWriter.SetText(body);
		senderLabel.text = ((!string.IsNullOrEmpty(sender)) ? Localization.Get(sender) : Localization.Get("UI_UNKNOWN"));
		dateLabel.text = StringFormatter.GetFormattedDate(date);
		mugshotTexture.mainTexture = portrait;
		this.yesCallback = yesCallback;
		this.noCallback = noCallback;
		yesButton.gameObject.SetActive(this.yesCallback != null);
		noButton.gameObject.SetActive(this.noCallback != null);
	}

	public void OnYesClicked()
	{
		Controller<AppController>().Back();
		if (yesCallback != null)
		{
			yesCallback();
		}
	}

	public void OnNoClicked()
	{
		Controller<AppController>().Back();
		if (noCallback != null)
		{
			noCallback();
		}
	}
}

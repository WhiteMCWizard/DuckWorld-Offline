using System;
using SLAM.Shared;
using UnityEngine;

namespace SLAM.Engine;

public class HubPremiumGameView : View
{
	[SerializeField]
	private UILabel title;

	[SerializeField]
	private UILabel body;

	[SerializeField]
	private UIButton okButton;

	[SerializeField]
	private UILabel okLabel;

	[SerializeField]
	private UIButton cancelButton;

	[SerializeField]
	private UILabel cancelLabel;

	[SerializeField]
	private UIGrid buttonGrid;

	[SerializeField]
	private UIEncapsulate windowContainer;

	[SerializeField]
	private UISprite screenshot;

	private Action okButtonHandler;

	private Action cancelButtonHandler;

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		AudioController.Play("Interface_window_open");
		windowContainer.Encapsulate();
	}

	public override void Close(Callback callback, bool immediately)
	{
		base.Close(callback, immediately);
		if (okButtonHandler != null || cancelButtonHandler != null)
		{
			AudioController.Play("Interface_window_close");
		}
		okButtonHandler = (cancelButtonHandler = null);
	}

	public void SetInfo(int gameId, string title, string message, string okButtonText, Action okButtonCallback)
	{
		SetInfo(gameId, title, message, okButtonText, null, okButtonCallback, null);
	}

	public void SetInfo(int gameId, string title, string message, string okButtonText, string cancelButtonText, Action okButtonCallback, Action cancelButtonCallback)
	{
		screenshot.spriteName = gameId.ToString();
		this.title.text = title;
		body.text = message;
		okLabel.text = okButtonText.PadRight(5);
		cancelLabel.text = cancelButtonText.PadLeft(5);
		okButtonHandler = okButtonCallback;
		cancelButtonHandler = cancelButtonCallback;
		okLabel.gameObject.SetActive(!string.IsNullOrEmpty(okButtonText));
		cancelLabel.gameObject.SetActive(!string.IsNullOrEmpty(cancelButtonText));
		if (cancelButtonText == null)
		{
			okLabel.pivot = UIWidget.Pivot.Center;
		}
		buttonGrid.enabled = true;
		buttonGrid.Reposition();
	}

	public void OnOkClicked()
	{
		if (okButtonHandler != null)
		{
			okButtonHandler();
		}
		Close();
	}

	public void OnCancelClicked()
	{
		if (cancelButtonHandler != null)
		{
			cancelButtonHandler();
		}
		Close();
	}

	public void OnCloseClicked()
	{
		Close();
	}
}

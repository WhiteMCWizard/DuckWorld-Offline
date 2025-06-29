using System;
using SLAM.Shared;
using UnityEngine;

namespace SLAM.Engine;

public class PopupView : View
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
	private UIButton closeButton;

	[SerializeField]
	private UIGrid buttonGrid;

	[SerializeField]
	private UIEncapsulate windowContainer;

	private Action okButtonHandler;

	private Action cancelButtonHandler;

	public override void Open(Callback callback, bool immediately)
	{
		base.Open(callback, immediately);
		AudioController.Play("Interface_window_open");
		Controller<PopupController>().SetUI(goState: true);
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
		Controller<PopupController>().SetUI(goState: false);
	}

	public void SetInfo(string title, string message, string okButtonText, Action okButtonCallback)
	{
		SetInfo(title, message, okButtonText, null, okButtonCallback, null);
	}

	public void SetInfo(string title, string message, string okButtonText, string cancelButtonText, Action okButtonCallback, Action cancelButtonCallback, bool closeButtonVisible = true)
	{
		this.title.text = title;
		body.text = message;
		okLabel.text = okButtonText.PadBoth(10);
		if (!string.IsNullOrEmpty(cancelButtonText))
		{
			okLabel.pivot = UIWidget.Pivot.Right;
			cancelLabel.text = cancelButtonText.PadBoth(10);
		}
		else
		{
			okLabel.pivot = UIWidget.Pivot.Center;
		}
		okButtonHandler = okButtonCallback;
		cancelButtonHandler = cancelButtonCallback;
		okLabel.gameObject.SetActive(!string.IsNullOrEmpty(okButtonText));
		cancelLabel.gameObject.SetActive(!string.IsNullOrEmpty(cancelButtonText));
		closeButton.gameObject.SetActive(closeButtonVisible);
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

using System;

namespace SLAM.Engine;

internal class PopupEvent
{
	public string Title;

	public string Message;

	public string OkButtonText;

	public string CancelButtonText;

	public Action OkButtonCallback;

	public Action CancelButtonCallback;

	public bool CloseButtonVisible;

	public PopupEvent(string title, string message, string okButtonText, Action okButtonCallback)
		: this(title, message, okButtonText, null, okButtonCallback, null, closeButtonVisible: true)
	{
	}

	public PopupEvent(string title, string message, string okButtonText, string cancelButtonText, Action okButtonCallback, Action cancelButtonCallback)
		: this(title, message, okButtonText, cancelButtonText, okButtonCallback, cancelButtonCallback, closeButtonVisible: true)
	{
	}

	public PopupEvent(string title, string message, string okButtonText, string cancelButtonText, Action okButtonCallback, Action cancelButtonCallback, bool closeButtonVisible)
	{
		Title = title;
		Message = message;
		OkButtonText = okButtonText;
		CancelButtonText = cancelButtonText;
		OkButtonCallback = okButtonCallback;
		CancelButtonCallback = cancelButtonCallback;
		CloseButtonVisible = closeButtonVisible;
	}
}

using UnityEngine;

namespace SLAM.Engine;

public class PopupController : ViewController
{
	[SerializeField]
	private PopupView popupView;

	protected override void Start()
	{
		base.Start();
		AddView(popupView);
		SetUI(goState: false);
	}

	private void OnEnable()
	{
		GameEvents.Subscribe<PopupEvent>(onPopupEvent);
	}

	private void OnDisable()
	{
		GameEvents.Unsubscribe<PopupEvent>(onPopupEvent);
	}

	private void onPopupEvent(PopupEvent evt)
	{
		popupView.SetInfo(evt.Title, evt.Message, evt.OkButtonText, evt.CancelButtonText, evt.OkButtonCallback, evt.CancelButtonCallback, evt.CloseButtonVisible);
		popupView.Open();
	}

	public void SetUI(bool goState)
	{
		base.transform.GetChild(0).gameObject.SetActive(goState);
	}
}

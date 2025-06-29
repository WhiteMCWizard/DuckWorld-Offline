using UnityEngine;

namespace SLAM.Smartphone;

public class ReceiveCallView : AppView
{
	[SerializeField]
	private UILabel nameLabel;

	[SerializeField]
	private UITexture portrait;

	public void SetData(string calledId, Texture2D mugshot)
	{
		nameLabel.text = calledId;
		portrait.mainTexture = mugshot;
	}

	public void OnAcceptCallClicked()
	{
		Controller<PhoneCallApp>().AcceptCall();
	}

	public void OnIgnoreCallClicked()
	{
		Controller<PhoneCallApp>().IgnoreCall();
	}
}

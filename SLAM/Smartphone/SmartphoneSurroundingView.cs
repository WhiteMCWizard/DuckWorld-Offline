using SLAM.Engine;
using UnityEngine;

namespace SLAM.Smartphone;

public class SmartphoneSurroundingView : View
{
	[SerializeField]
	private UIButton closeButton;

	public void SetData(bool canUserClose)
	{
		closeButton.gameObject.SetActive(canUserClose);
	}

	public void OnCloseClicked()
	{
		Controller<SmartphoneController>().Hide();
	}
}

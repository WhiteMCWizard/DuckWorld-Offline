using SLAM.Engine;
using UnityEngine;

namespace SLAM.Shops;

public class HUDView : View
{
	[SerializeField]
	private UILabel cashLabel;

	public void SetInfo(int cashLeft)
	{
		cashLabel.text = cashLeft.ToString();
	}

	public void OnHomeClicked()
	{
		Controller<InventoryController>().GoToHub();
	}
}

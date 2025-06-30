using SLAM.Engine;
using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class ChaseTheBoatHUDView : HUDView
{
	[SerializeField]
	private UILabel feathersLabel;

	public void UpdateFeathers(int amount)
	{
		feathersLabel.text = amount.ToString();
	}
}

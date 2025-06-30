using SLAM.Engine;
using UnityEngine;

namespace SLAM.BatCave;

public class BC_HUDView : HUDView
{
	[SerializeField]
	private UILabel exoticAnimalCount;

	public void SetInfo(int exoticAnimalCount)
	{
		this.exoticAnimalCount.text = $"x{exoticAnimalCount}";
	}
}

using SLAM.Engine;
using UnityEngine;

namespace SLAM.KartRacing;

public class KRhudView : HUDView
{
	[SerializeField]
	private UILabel position;

	public void UpdatePosition(int myPos, int totalPositions)
	{
		position.text = myPos + "/" + totalPositions;
	}
}

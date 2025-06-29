using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPTutorialView02 : TutorialView
{
	[SerializeField]
	private ToolTip pipeToolTip;

	protected override void Start()
	{
		base.Start();
		pipeToolTip.Show();
	}
}

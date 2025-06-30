using SLAM.Engine;
using SLAM.ToolTips;
using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPTutorialView03 : TutorialView
{
	[SerializeField]
	private ToolTip pipeToolTip01;

	[SerializeField]
	private ToolTip pipeToolTip02;

	protected override void Start()
	{
		base.Start();
		pipeToolTip01.Show();
		pipeToolTip02.Show();
	}
}

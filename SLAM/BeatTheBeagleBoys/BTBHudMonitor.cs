using UnityEngine;

namespace SLAM.BeatTheBeagleBoys;

public class BTBHudMonitor : BTBMonitor
{
	[SerializeField]
	private BTBCamera targetCamera;

	private void Start()
	{
		base.currentCamera = targetCamera;
		stateMachine.SwitchTo("Active");
	}
}

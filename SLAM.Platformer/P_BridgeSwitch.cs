using UnityEngine;

namespace SLAM.Platformer;

public class P_BridgeSwitch : P_Toggle
{
	[SerializeField]
	private P_Bridge bridge;

	protected override void OnTogglePressed(bool state)
	{
		bridge.Toggle(state);
	}
}

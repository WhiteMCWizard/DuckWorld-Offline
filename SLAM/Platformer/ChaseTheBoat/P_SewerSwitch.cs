using UnityEngine;

namespace SLAM.Platformer.ChaseTheBoat;

public class P_SewerSwitch : P_Toggle
{
	[SerializeField]
	private CTB_Sewer sewer;

	protected override void OnTogglePressed(bool state)
	{
		sewer.Turn(state);
	}
}

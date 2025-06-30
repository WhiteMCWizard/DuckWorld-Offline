using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPEndPipe : CTPPipe
{
	private void Reset()
	{
		canRotate = false;
	}

	public override bool CanFlowWater(Vector3 otherInDir, out Vector3 otherOutDir)
	{
		otherOutDir = Vector3.zero;
		if (HasWaterInTube())
		{
			return false;
		}
		waterInTube = true;
		return true;
	}
}

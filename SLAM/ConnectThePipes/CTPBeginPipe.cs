using UnityEngine;

namespace SLAM.ConnectThePipes;

public class CTPBeginPipe : CTPPipe
{
	[SerializeField]
	private Transform avatarWalkToObject;

	public Transform AvatarWalkToObject => avatarWalkToObject;

	private void Reset()
	{
		canRotate = false;
	}

	public override bool CanFlowWater(Vector3 otherInDir, out Vector3 outDirection)
	{
		if (waterInTube)
		{
			outDirection = Vector3.zero;
			return false;
		}
		float a = Vector3.Dot(base.transform.TransformDirection(inDirection), otherInDir);
		waterInTube = true;
		if (Mathf.Approximately(a, 1f))
		{
			outDirection = base.transform.TransformDirection(base.outDirection);
		}
		else
		{
			outDirection = -base.transform.TransformDirection(inDirection);
		}
		return true;
	}

	public override void OnClick(int direction)
	{
		Object.FindObjectOfType<ConnectThePipesGame>().StartWaterFlow();
	}
}

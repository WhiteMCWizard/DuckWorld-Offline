using UnityEngine;

namespace SLAM.Platformer;

[RequireComponent(typeof(BoxCollider))]
public class P_Ladder : P_PressUpTrigger
{
	protected override UpAction DoAction()
	{
		Vector3 position = base.Player.transform.position;
		position.x = base.transform.position.x;
		base.Player.transform.position = position;
		return new UpAction(Action.Climbing, 0f, this);
	}
}
